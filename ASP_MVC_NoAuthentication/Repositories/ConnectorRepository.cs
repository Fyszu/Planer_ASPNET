using ASP_MVC_NoAuthentication.Data;
using Microsoft.EntityFrameworkCore;

namespace ASP_MVC_NoAuthentication.Repositories
{
    public class ConnectorRepository : IConnectorRepository
    {
        private readonly MyDbContext _context;
        public ConnectorRepository(MyDbContext context)
        {
            _context = context;
        }



        public async Task<Connector> GetById(int id)
        {
            return await _context.Connectors.FindAsync(id);
        }

        public async Task Add(Connector connector)
        {
            if(connector != null)
                await _context.AddAsync(connector);
            await _context.SaveChangesAsync();
        }

        public async Task Update(Connector connector)
        {
            if (connector != null)
            {
                Connector dbConnector = await GetById(connector.Id);
                dbConnector.ChargingPoints = connector.ChargingPoints;
                dbConnector.Name = connector.Name;
            }
            await _context.SaveChangesAsync();
        }

        public async Task Remove(Connector connector)
        {
            if(connector != null)
                _context.Remove(connector);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Connector>> GetAll()
        {
            return await _context.Connectors.ToListAsync();
        }
    }
}
