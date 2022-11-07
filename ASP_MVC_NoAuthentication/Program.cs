using ASP_MVC_NoAuthentication.Data;
using ASP_MVC_NoAuthentication.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.Certificate;
using System.Text.Json.Serialization;
using ASP_MVC_NoAuthentication.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<MyDbContext>(options => options.UseMySQL(builder.Configuration.GetConnectionString("Default")));
builder.Services.AddAuthentication(CertificateAuthenticationDefaults.AuthenticationScheme).AddCertificate();
builder.Services.AddIdentity<User, IdentityRole>(
    options => {
        options.SignIn.RequireConfirmedAccount = false;
        options.Password.RequireDigit = false;
        options.Password.RequireLowercase = false;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireUppercase = false;
        options.Password.RequiredLength = 6;
        options.Password.RequiredUniqueChars = 0;
        }
    ).AddEntityFrameworkStores<MyDbContext>();
builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.HttpOnly = true;

// Remember me expire time
    options.ExpireTimeSpan = TimeSpan.FromDays(30);
    options.Cookie.Name = "Planer.AuthCookieAspNetCore";

    // Redirection settings
    options.LoginPath = "/Identity/Account/Login";
    options.LogoutPath = "/Identity/Account/Logout";
    options.AccessDeniedPath = "/Identity/Account/AccessDenied";
    options.SlidingExpiration = true;
});

// Logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

// Services and repositories
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddScoped<IGeoService, GeoService>();
builder.Services.AddScoped<IChargingStationService, ChargingStationService>();
builder.Services.AddScoped<IDistanceService, DistanceService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IConnectorInterfaceService, ConnectorInterfaceService>();
builder.Services.AddScoped<ICarService, CarService>();
builder.Services.AddScoped<IWeatherAPIService, WeatherAPIService>();
builder.Services.AddScoped<CarRepository, CarRepository>();
builder.Services.AddScoped<ConnectorInterfaceRepository, ConnectorInterfaceRepository>();
builder.Services.AddScoped<UserRepository, UserRepository>();
builder.Services.AddScoped<ChargingStationsRepository, ChargingStationsRepository>();
builder.Services.AddControllersWithViews().AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
builder.Services.AddRazorPages();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

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
