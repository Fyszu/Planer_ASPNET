using RoutePlanner.Data;
using RoutePlanner.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.Certificate;
using System.Text.Json.Serialization;
using RoutePlanner.Repositories;
using AspNetCoreRateLimit;
using RoutePlanner.Filters;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<MyDbContext>(options => options.UseMySQL(builder.Configuration.GetConnectionString("Default")));
builder.Services.AddAuthentication(CertificateAuthenticationDefaults.AuthenticationScheme).AddCertificate();

// Identity library accounts
builder.Services.AddIdentity<User, IdentityRole>(
    options => {
        // Register requirements
        options.SignIn.RequireConfirmedAccount = false;
        options.User.RequireUniqueEmail = true;

        // Password requirements
        options.Password.RequireDigit = false;
        options.Password.RequireLowercase = false;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireUppercase = true;
        options.Password.RequiredLength = 8;
        options.Password.RequiredUniqueChars = 1;

        // Login attempts - lock after too many wrong attempts
        options.Lockout.AllowedForNewUsers = true;
        options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(65);
        options.Lockout.MaxFailedAccessAttempts = 4;
        }
    ).AddEntityFrameworkStores<MyDbContext>();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.HttpOnly = true;

    // Remember me expire time
    options.ExpireTimeSpan = TimeSpan.FromDays(30);
    options.Cookie.Name = "Planer.AuthCookieAspNetCore";

    // Redirection settings
    options.LoginPath = "/Login";
    options.LogoutPath = "/Logout";
    options.AccessDeniedPath = "/AccessDenied";
    options.SlidingExpiration = true;
});

// Logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.SetMinimumLevel(LogLevel.Trace);

// Services
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddScoped<IGeoService, GeoService>();
builder.Services.AddScoped<IChargingStationService, ChargingStationService>();
builder.Services.AddScoped<IDistanceService, DistanceService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IConnectorInterfaceService, ConnectorInterfaceService>();
builder.Services.AddScoped<ICarService, CarService>();
builder.Services.AddScoped<IWeatherApiService, WeatherApiService>();

// Repositories
builder.Services.AddScoped<ICarRepository, CarRepository>();
builder.Services.AddScoped<IConnectorInterfaceRepository, ConnectorInterfaceRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IChargingStationsRepository, ChargingStationsRepository>();

// Ingore cycles - ingore reference looping while JSON serializing
builder.Services.AddControllers().AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

// Throttle rate limit - services
builder.Services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
builder.Services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
builder.Services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
builder.Services.AddSingleton<IProcessingStrategy, AsyncKeyLockProcessingStrategy>();
builder.Services.AddInMemoryRateLimiting();

// Throttle rate limit - configuration for controllers
builder.Services.Configure<IpRateLimitOptions>(options =>
{
    options.EnableEndpointRateLimiting = true;
    options.StackBlockedRequests = false;
    options.HttpStatusCode = 429;
    options.RealIpHeader = "X-Real-IP";
    options.ClientIdHeader = "X-ClientId";
    options.GeneralRules = new List<RateLimitRule>
        {
            // GeoController -> GetCoordinates
            new RateLimitRule
            {
                Endpoint = "GET:/geo/GetCoordinates",
                Period = "15s",
                Limit = 6,
            },
            new RateLimitRule
            {
                Endpoint = "GET:/geo/GetCoordinates",
                Period = "1h",
                Limit = 200,
            },

            // GeoController -> GetAddress
            new RateLimitRule
            {
                Endpoint = "*:/geo/GetAddress",
                Period = "15s",
                Limit = 3,
            },
            new RateLimitRule
            {
                Endpoint = "*:/geo/GetAddress",
                Period = "1h",
                Limit = 100,
            },

            // DistanceController -> GetRealDistance
            new RateLimitRule
            {
                Endpoint = "*:/distance/GetRealDistance",
                Period = "15s",
                Limit = 3,
            },
            new RateLimitRule
            {
                Endpoint = "*:/distance/GetRealDistance",
                Period = "1h",
                Limit = 100,
            },

            // CarController -> *
            new RateLimitRule
            {
                Endpoint = "*:/car/*",
                Period = "10s",
                Limit = 10,
            },

            // UserController -> *
            new RateLimitRule
            {
                Endpoint = "*:/user/*",
                Period = "10s",
                Limit = 10,
            }
        };
});

builder.Services.AddRazorPages();

var app = builder.Build();

// Throttle rate limit - turn on in application
app.UseMiddleware<InternalIPRateLimitMiddleware>();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseExceptionHandler("/Error");
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseCookiePolicy();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
    );

app.MapRazorPages();

app.Run();
