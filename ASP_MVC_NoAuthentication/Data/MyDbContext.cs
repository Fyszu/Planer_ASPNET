using Microsoft.EntityFrameworkCore;

namespace ASP_MVC_NoAuthentication.Data
{
    public class MyDbContext : DbContext
    {
        public MyDbContext() { }
        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options)
        {

        }
        public DbSet<User> Users { get; set; }
        public DbSet<ChargingPoint> ChargingPoints {get;set;}
        public DbSet<ChargingStation> ChargingStations { get; set;}
        public DbSet<Car> Cars { get; set; }
        public DbSet<Connector> Connectors { get; set; }
        public DbSet<PaymentMethod> PaymentMethods { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                IConfigurationRoot configuration = new ConfigurationBuilder()
                   .SetBasePath(Directory.GetCurrentDirectory())
                   .AddJsonFile("appsettings.json")
                   .Build();
                var connectionString = configuration.GetConnectionString("Default");
                optionsBuilder.UseMySQL(connectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
