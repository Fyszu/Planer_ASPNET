using ASP_MVC_NoAuthentication.Data;
using ASP_MVC_NoAuthentication.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EipaUdtImportingTools.Tools
{
    internal static class UDTApiDatabaseUpdater
    {
        public static List<ChargingStation> ChargingStations { get; set; }
        public static HashSet<ConnectorInterface> ConnectorInterfacesInUsage { get; set; }
        public static async Task<bool> UpdateDatabase(MyDbContext dbContext, ChargingStationsRepository chargingStationsRepository, ChargingPointsRepository chargingPointsRepository, ProvidersRepository providersRepository, ConnectorInterfaceRepository connectorInterfaceRepository, CarRepository carRepository)
        {
            try
            {
                if (chargingStationsRepository == null) throw new Exception("Wystąpił błąd - przekazane repozytorium stacji ładowania jest nullem.");
                if (chargingPointsRepository == null) throw new Exception("Wystąpił błąd - przekazane  puntków ładowania jest nullem.");
                if (providersRepository == null) throw new Exception("Wystąpił błąd - przekazane repozytorium operatorów jest nullem.");
                if (connectorInterfaceRepository == null) throw new Exception("Wystąpił błąd - przekazane repozytorium interfejsów jest nullem.");

                Stopwatch generalStopwatch = new();
                Stopwatch stopwatch = new();
                generalStopwatch.Start();
                stopwatch.Start();

                if (await ApiImporter.TransformApiDataToInternal())
                {
                    stopwatch.Stop();
                    Console.WriteLine($"Czas konwersji danych: {stopwatch.ElapsedMilliseconds}ms");

                    if (ChargingStations.Count == 0) throw new Exception("Lista stacji ładowania jest pusta.");
                    if (ConnectorInterfacesInUsage.Count == 0) throw new Exception("Zbiór nowych interfejsów jest pusty.");

                    stopwatch.Restart();

                    List<Car> cars = await carRepository.GetAll() ?? throw new Exception("Problem z pobraniem listy samochodów.");
                    if (cars.Count == 0) throw new Exception("Lista samochodów jest pusta.");

                    stopwatch.Stop();
                    Console.WriteLine($"Czas pobrania listy samochodów: {stopwatch.ElapsedMilliseconds}ms");
                    stopwatch.Restart();

                    // Begin transaction
                    using var transaction = await dbContext.Database.BeginTransactionAsync();

                    // Remove old data from DB
                    await chargingPointsRepository.RemoveAll();
                    await chargingStationsRepository.RemoveAll();
                    await connectorInterfaceRepository.RemoveAll();
                    await providersRepository.RemoveAll();
                    await carRepository.RemoveAll();

                    stopwatch.Stop();
                    Console.WriteLine($"Czas usunięcia danych z bazy danych: {stopwatch.ElapsedMilliseconds}ms");
                    stopwatch.Restart();

                    // Add new data to DB
                    await chargingStationsRepository.AddRange(ChargingStations);

                    stopwatch.Stop();
                    Console.WriteLine($"Czas aktualizacji bazy ładowarek i tabel referencyjnych: {stopwatch.ElapsedMilliseconds}ms.");
                    stopwatch.Restart();

                    // Update cars connector interfaces
                    cars = RefreshCarsConnectorInterfaces(cars);

                    // Add cars with resolved interfaces to DB
                    await carRepository.AddRange(cars);

                    stopwatch.Stop();
                    Console.WriteLine($"Czas aktualizacji samochodów: {stopwatch.ElapsedMilliseconds}ms.");

                    // End of transaction
                    await transaction.CommitAsync();

                    generalStopwatch.Stop();
                    Console.WriteLine($"Czas wykonania całej transakcji: {generalStopwatch.ElapsedMilliseconds}ms.");

                    return true;
                }
                else
                {
                    throw new Exception("Aktualizacja nie może być kontynuowana.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Problem podczas zapisu do bazy danych: ");
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.InnerException);
                return false;
            }
        }

        private static List<Car> RefreshCarsConnectorInterfaces(List<Car> cars)
        {
            // Dictionary of connector interfaces, that have changed in compare of previous database data
            Dictionary<KeyValuePair<ConnectorInterface, ConnectorInterface>, ConnectorInterfaceChange> connectorInterfacesWhichHaveChanged = new();

            // Resolve new connector interfaces for all existing cars and remove interfaces from cars, which don't exist anymore
            foreach (Car car in cars)
            {
                List<ConnectorInterface> newCarInterfaces = new();
                foreach (var oldConnectorInterface in car.ConnectorInterfaces)
                {
                    bool removed = true;
                    foreach (var newConnectorInterface in ConnectorInterfacesInUsage)
                    {
                        if (oldConnectorInterface.Name.Equals(newConnectorInterface.Name))
                        {
                            newCarInterfaces.Add(newConnectorInterface);
                            if (!oldConnectorInterface.Description.Equals(newConnectorInterface.Description))
                                connectorInterfacesWhichHaveChanged.Add(KeyValuePair.Create(oldConnectorInterface, newConnectorInterface), ConnectorInterfaceChange.Description);
                            if (oldConnectorInterface.Id != newConnectorInterface.Id)
                                connectorInterfacesWhichHaveChanged.Add(KeyValuePair.Create(oldConnectorInterface, newConnectorInterface), ConnectorInterfaceChange.Id);
                            removed = false;
                            break;
                        }
                        else if (oldConnectorInterface.Id == newConnectorInterface.Id)
                        {
                            newCarInterfaces.Add(newConnectorInterface);
                            connectorInterfacesWhichHaveChanged.Add(KeyValuePair.Create(oldConnectorInterface, newConnectorInterface), ConnectorInterfaceChange.Name);
                            if (!oldConnectorInterface.Description.Equals(newConnectorInterface.Description))
                                connectorInterfacesWhichHaveChanged.Add(KeyValuePair.Create(oldConnectorInterface, newConnectorInterface), ConnectorInterfaceChange.Description);
                            removed = false;
                            break;
                        }
                    }
                    if (removed)
                        connectorInterfacesWhichHaveChanged.Add(KeyValuePair.Create(oldConnectorInterface, new ConnectorInterface()), ConnectorInterfaceChange.Removed);
                }
                car.ConnectorInterfaces = newCarInterfaces;
            }

            // Inform about changed connector interfaces in new set of data, if any
            foreach (var changedConnectorInterface in connectorInterfacesWhichHaveChanged)
            {
                switch (changedConnectorInterface.Value)
                {
                    case ConnectorInterfaceChange.Id:
                        Console.WriteLine($"Uwaga: interfejs ładowania {changedConnectorInterface.Key.Key} zmienił wartość ID na {changedConnectorInterface.Key.Value.Id}.");
                        break;

                    case ConnectorInterfaceChange.Name:
                        Console.WriteLine($"Uwaga: interfejs ładowania {changedConnectorInterface.Key.Key} zmienił nazwę na {changedConnectorInterface.Key.Value.Name}. Dopasowano po ID, lecz istnieje możliwość błędu.");
                        break;

                    case ConnectorInterfaceChange.Description:
                        Console.WriteLine($"Uwaga: interfejs ładowania {changedConnectorInterface.Key.Key} zmienił opis na {changedConnectorInterface.Key.Value.Description}.");
                        break;

                    case ConnectorInterfaceChange.Removed:
                        Console.WriteLine($"\n-------\nUWAGA: interfejs ładowania {changedConnectorInterface.Key.Key} został usunięty z bazy danych.\n--------\n");
                        break;

                    default:
                        throw new Exception("Błąd podczas przekazywania informacji o zmienionych interfejsach ładowania.");
                }
            }

            return cars;
        }

        private enum ConnectorInterfaceChange
        {
            Id,
            Name,
            Description,
            Removed
        }
    }
}
