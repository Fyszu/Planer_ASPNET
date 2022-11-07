using ASP_MVC_NoAuthentication.Data;
using Microsoft.EntityFrameworkCore;

namespace ASP_MVC_NoAuthentication.Repositories
{
    public class ChargingPointsRepository : IChargingPointsRepository
    {
        private readonly MyDbContext _context;
        public ChargingPointsRepository(MyDbContext context)
        {
            _context = context;
        }

        public async Task Add(ChargingPoint entity)
        {
            if (entity != null)
            {
                await _context.ChargingPoints.AddAsync(entity);
                await _context.SaveChangesAsync();
            }
            else
                throw new Exception("Błąd podczas dodawania punktu ładowania. Przekazany punkt ładowania jest nullem.");
        }

        public async Task<List<ChargingPoint>> GetAll()
        {
            return await _context.ChargingPoints.Include(point => point.Connectors).ToListAsync();
        }

        public async Task<ChargingPoint> GetById(long id)
        {
            return await _context.ChargingPoints.Include(chargingPoint => chargingPoint.Connectors).Where(chargingPoint => chargingPoint.Id == id).SingleOrDefaultAsync();
        }

        public async Task Remove(ChargingPoint entity)
        {
            if (entity != null)
            {
                _context.ChargingPoints.Remove(entity);
                await _context.SaveChangesAsync();
            }
            else
                throw new Exception("Błąd podczas usuwania punktu ładowania. Przekazany punkt ładowania jest nullem.");
        }

        // Removes all records from ChargingPoints table
        public async Task RemoveAll() 
        {
            // Remove also connectors
            await _context.Connector.ForEachAsync(connector =>
            {
                if (connector != null)
                    _context.Connector.Remove(connector);
            });

            await _context.SaveChangesAsync();

            await _context.ChargingPoints.Include(point => point.Connectors).ForEachAsync(point => {
                if (point != null)
                    _context.ChargingPoints.Remove(point);
            });

            await _context.SaveChangesAsync();
        }

        public async Task Update(ChargingPoint entity)
        {
            if (entity != null)
            {
                ChargingPoint dbChargingPoint = await GetById(entity.Id);
                if (dbChargingPoint != null)
                {
                    dbChargingPoint.Station = entity.Station;
                    dbChargingPoint.Status = entity.Status;
                    dbChargingPoint.Connectors = entity.Connectors;
                    dbChargingPoint.ChargingModes = entity.ChargingModes;
                    dbChargingPoint.Price = entity.Price;
                    dbChargingPoint.PriceUnit = entity.PriceUnit;
                    await _context.SaveChangesAsync();
                }
                else
                    throw new Exception("Błąd podczas aktualizacji punktu ładowania. Nie znaleziono punktu o ID: " + entity.Id);
            }
            else
                throw new Exception("Błąd podczas aktualizacji punktu ładowania. Przekazany punkt ładowania jest nullem.");
        }
    }
}
