using ASP_MVC_NoAuthentication.Data;
using Microsoft.EntityFrameworkCore;

namespace ASP_MVC_NoAuthentication.Repositories
{
    public class ChargingStationsRepository : IChargingStationsRepository
    {
        private readonly MyDbContext _context;
        public ChargingStationsRepository(MyDbContext context)
        {
            _context = context;
        }



        public ChargingStation GetById(int id)
        {
            return _context.ChargingStations.Include(station => station.ChargingPoints).ThenInclude(point => point.Connector).Where(station => station.Id.Equals(id)).SingleOrDefault();
        }

        public void Add(ChargingStation chargingStation)
        {
            if(chargingStation != null)
                _context.ChargingStations.Add(chargingStation);
            _context.SaveChanges();
        }

        public void Update(ChargingStation chargingStation)
        {
            if (chargingStation != null)
            {
                ChargingStation dbChargingStation = GetById(chargingStation.Id);
                dbChargingStation.Street = chargingStation.Street;
                dbChargingStation.ChargingPoints = chargingStation.ChargingPoints;
                dbChargingStation.PaymentMethods = chargingStation.PaymentMethods;
                dbChargingStation.StationState = chargingStation.StationState;
                dbChargingStation.Latitude = chargingStation.Latitude;
                dbChargingStation.Longitude = chargingStation.Longitude;
                dbChargingStation.OpenHours = chargingStation.OpenHours;
                dbChargingStation.City = chargingStation.City;
                dbChargingStation.Owner = chargingStation.Owner;
                dbChargingStation.Name = chargingStation.Name;
                dbChargingStation.PostalAdress = chargingStation.PostalAdress;
            }
            _context.SaveChanges();
        }

        public void Remove(ChargingStation chargingStation)
        {
            if (chargingStation != null)
                _context.ChargingStations.Remove(chargingStation);
            _context.SaveChanges();
        }

        public List<ChargingStation> GetAll()
        {
            return _context.ChargingStations.Include(station => station.ChargingPoints).ThenInclude(point => point.Connector).ToList();
        }
    }
}
