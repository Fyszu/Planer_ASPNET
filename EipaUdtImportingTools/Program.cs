using ASP_MVC_NoAuthentication.Data;
using ASP_MVC_NoAuthentication.Repositories;
using ASP_MVC_NoAuthentication.Services;
using EipaUdtImportingTools.Tools;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

var builder = WebApplication.CreateBuilder(args);

// DB Context
builder.Services.AddDbContext<MyDbContext>(options => {
    options.UseMySQL(builder.Configuration.GetConnectionString("Default"));
    options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
    });

// Logging - filter sql queries by EF in appsettings.json
ApplicationLogging.LoggerFactory = LoggerFactory.Create(
        loggingBuilder => loggingBuilder.AddConsole()
        .SetMinimumLevel(LogLevel.Trace)
    );

// Services
builder.Services.AddScoped<ChargingStationsRepository, ChargingStationsRepository>();
builder.Services.AddScoped<ChargingPointsRepository, ChargingPointsRepository>();
builder.Services.AddScoped<ProvidersRepository, ProvidersRepository>();
builder.Services.AddScoped<ConnectorInterfaceRepository, ConnectorInterfaceRepository>();
builder.Services.AddScoped<CarRepository, CarRepository>();

var provider = builder.Services.BuildServiceProvider();

// Run tools
bool result = false;
int retryCount = 3;

for(int i = 0; i < retryCount; i++)
{
    Console.WriteLine($"\n\n--------------------------------\nPróba aktualizacji bazy danych numer {i + 1}\n--------------------------------\n");
    result = await UDTApiDatabaseUpdater.UpdateDatabase(provider.GetService<MyDbContext>(), provider.GetService<ChargingStationsRepository>(), provider.GetService<ChargingPointsRepository>(), provider.GetService<ProvidersRepository>(), provider.GetService<ConnectorInterfaceRepository>(), provider.GetService<CarRepository>());
    if (result) break;
}




// Logger
internal static class ApplicationLogging
{
    internal static ILoggerFactory LoggerFactory { get; set; }
    internal static ILogger CreateLogger<T>() => LoggerFactory.CreateLogger<T>();
    internal static ILogger CreateLogger(string categoryName) => LoggerFactory.CreateLogger(categoryName);
}