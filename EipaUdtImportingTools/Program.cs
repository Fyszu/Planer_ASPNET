using ASP_MVC_NoAuthentication.Data;
using ASP_MVC_NoAuthentication.Repositories;
using ASP_MVC_NoAuthentication.Services;
using EipaUdtImportingTools.Tools;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<MyDbContext>(options => {
    options.UseMySQL(builder.Configuration.GetConnectionString("Default"));
    options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
    options.EnableSensitiveDataLogging();
    });
builder.Services.AddScoped<ChargingStationsRepository, ChargingStationsRepository>();
builder.Services.AddScoped<ChargingPointsRepository, ChargingPointsRepository>();
builder.Services.AddScoped<ProvidersRepository, ProvidersRepository>();
builder.Services.AddScoped<ConnectorInterfaceRepository, ConnectorInterfaceRepository>();
builder.Services.AddScoped<CarRepository, CarRepository>();
var provider = builder.Services.BuildServiceProvider();

bool result = false;
int retryCount = 1;

for(int i = 0; i < retryCount; i++)
{
    Console.WriteLine($"Próba aktualizacji bazy danych numer {i + 1}");
    result = await UDTApiDatabaseUpdater.UpdateDatabase(provider.GetService<MyDbContext>(), provider.GetService<ChargingStationsRepository>(), provider.GetService<ChargingPointsRepository>(), provider.GetService<ProvidersRepository>(), provider.GetService<ConnectorInterfaceRepository>(), provider.GetService<CarRepository>());
    if (result) break;
}

if (result)
    Console.WriteLine("Baza danych została zaktualizowana.");
else
    Console.WriteLine("Aktualizacja bazy danych zakończona niepowodzeniem.");