using RoutePlanner.Data;
using RoutePlanner.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EipaUdtImportingTools.Tools
{
    internal static class DatabaseUpdater
    {
        private static readonly ILogger logger = ApplicationLogging.CreateLogger("DatabaseUpdater");
        public static HashSet<ChargingStation> ChargingStations { get; set; }
        public static HashSet<ConnectorInterface> ConnectorInterfacesInUsage { get; set; }
        public static async Task<bool> UpdateDatabase(MyDbContext dbContext, IChargingStationsRepository chargingStationsRepository, IChargingPointsRepository chargingPointsRepository, IProvidersRepository providersRepository, IConnectorInterfaceRepository connectorInterfaceRepository, ICarRepository carRepository)
        {
            try
            {
                if (chargingStationsRepository == null) throw new ArgumentNullException("Wystąpił błąd - przekazane repozytorium stacji ładowania jest nullem.");
                if (chargingPointsRepository == null) throw new ArgumentNullException("Wystąpił błąd - przekazane  puntków ładowania jest nullem.");
                if (providersRepository == null) throw new ArgumentNullException("Wystąpił błąd - przekazane repozytorium operatorów jest nullem.");
                if (connectorInterfaceRepository == null) throw new ArgumentNullException("Wystąpił błąd - przekazane repozytorium interfejsów jest nullem.");
                
                Stopwatch generalStopwatch = new();
                Stopwatch stopwatch = new();
                generalStopwatch.Start();
                stopwatch.Start();
                
                if (await EipaConverter.TransformApiDataToInternal())
                {
                    stopwatch.Stop();
                    logger.LogInformation($"Czas konwersji danych: {stopwatch.ElapsedMilliseconds}ms");

                    if (ChargingStations.Count == 0) throw new ArgumentException("Lista stacji ładowania jest pusta.");
                    if (ConnectorInterfacesInUsage.Count == 0) throw new ArgumentException("Zbiór nowych interfejsów jest pusty.");

                    stopwatch.Restart();

                    logger.LogTrace("Pobieranie danych samochodów.");
                    List<Car> cars = await carRepository.GetAllAsync() ?? throw new ArgumentNullException("Problem z pobraniem listy samochodów.");
                    if (cars.Count == 0) throw new ArgumentException("Lista samochodów jest pusta.");

                    stopwatch.Stop();
                    logger.LogInformation($"Czas pobrania listy samochodów: {stopwatch.ElapsedMilliseconds}ms");
                    stopwatch.Restart();

                    // Begin transaction
                    logger.LogTrace("Rozpoczynanie transakcji.");
                    using var transaction = await dbContext.Database.BeginTransactionAsync();

                    // Remove old data from DB
                    logger.LogTrace("Usuwanie danych z bazy danych.");
                    await chargingPointsRepository.RemoveAllAsync();
                    await chargingStationsRepository.RemoveAllAsync();
                    await connectorInterfaceRepository.RemoveAllAsync();
                    await providersRepository.RemoveAllAsync();
                    await carRepository.RemoveAllAsync();

                    stopwatch.Stop();
                    logger.LogInformation($"Czas usunięcia danych z bazy danych: {stopwatch.ElapsedMilliseconds}ms");
                    stopwatch.Restart();

                    // Add new data to DB
                    logger.LogTrace("Dodawanie nowych danych do bazy danych.");
                    await chargingStationsRepository.AddRangeAsync(ChargingStations);

                    stopwatch.Stop();
                    logger.LogInformation($"Czas aktualizacji bazy ładowarek i tabel referencyjnych: {stopwatch.ElapsedMilliseconds}ms.");
                    stopwatch.Restart();

                    // Update cars connector interfaces
                    logger.LogTrace("Odświeżanie interfejsów ładowania w samochodach.");
                    cars = RefreshCarsConnectorInterfaces(cars);

                    // Add cars with resolved interfaces to DB
                    logger.LogTrace("Dodawanie samochodów do bazy danych.");
                    await carRepository.AddRangeAsync(cars);

                    stopwatch.Stop();
                    logger.LogInformation($"Czas aktualizacji samochodów: {stopwatch.ElapsedMilliseconds}ms.");

                    // End of transaction
                    await transaction.CommitAsync();
                    logger.LogTrace("Koniec transakcji.");

                    generalStopwatch.Stop();
                    logger.LogInformation($"Czas wykonania całej operacji: {generalStopwatch.ElapsedMilliseconds}ms.");

                    logger.LogInformation("\n\n--------------------------------\nBaza danych została zaktualizowana.\n--------------------------------");
                    return true;
                }
                else
                {
                    throw new ArgumentException("Aktualizacja nie może być kontynuowana.");
                }
            }
            catch (Exception ex)
            {
                logger.LogCritical($"Problem podczas zapisu do bazy danych: \n{ex.Message}\n{ex.InnerException}");
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
                HashSet<ConnectorInterface> newCarInterfaces = new();
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
                        logger.LogWarning($"Interfejs ładowania {changedConnectorInterface.Key.Key} zmienił wartość ID na {changedConnectorInterface.Key.Value.Id}.");
                        break;

                    case ConnectorInterfaceChange.Name:
                        logger.LogWarning($"Interfejs ładowania {changedConnectorInterface.Key.Key} zmienił nazwę na {changedConnectorInterface.Key.Value.Name}. Dopasowano po ID, lecz istnieje możliwość błędu.");
                        break;

                    case ConnectorInterfaceChange.Description:
                        logger.LogWarning($"Interfejs ładowania {changedConnectorInterface.Key.Key} zmienił opis na {changedConnectorInterface.Key.Value.Description}.");
                        break;

                    case ConnectorInterfaceChange.Removed:
                        logger.LogWarning($"Interfejs ładowania {changedConnectorInterface.Key.Key} został usunięty z bazy danych.");
                        break;

                    default:
                        throw new ArgumentException($"Błąd podczas przekazywania informacji o zmienionych interfejsach ładowania: {changedConnectorInterface.Value}");
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
