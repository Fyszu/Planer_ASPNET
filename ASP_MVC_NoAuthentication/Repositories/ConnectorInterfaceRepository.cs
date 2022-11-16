using ASP_MVC_NoAuthentication.Data;
using Microsoft.EntityFrameworkCore;

namespace ASP_MVC_NoAuthentication.Repositories
{
    public class ConnectorInterfaceRepository : IConnectorInterfaceRepository
    {
        private readonly MyDbContext _context;
        public ConnectorInterfaceRepository(MyDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(ConnectorInterface connectorInterface)
        {
            if (connectorInterface != null)
                await _context.ConnectorInterfaces.AddAsync(connectorInterface);
            await _context.SaveChangesAsync();
        }
        public List<ConnectorInterface> GetAll()
        {
            return _context.ConnectorInterfaces.ToList();
        }
        public async Task<List<ConnectorInterface>> GetAllAsync()
        {
            return await _context.ConnectorInterfaces.ToListAsync();
        }

        public async Task<ConnectorInterface> GetByIdAsync(long id)
        {
            return await _context.ConnectorInterfaces.Where(cni => cni.Id.Equals(id)).SingleOrDefaultAsync();
        }

        public async Task RemoveAsync(ConnectorInterface connectorInterface)
        {
            if (connectorInterface != null)
                _context.ConnectorInterfaces.Remove(connectorInterface);
            await _context.SaveChangesAsync();
        }

        // Removes all records from ConnectorInterfaces table
        public async Task RemoveAllAsync()
        {
            await _context.ConnectorInterfaces.ForEachAsync(cInterface => {
                if (cInterface != null)
                    _context.ConnectorInterfaces.Remove(cInterface);
            });
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(ConnectorInterface connectorInterface)
        {
            if (connectorInterface != null)
            {
                ConnectorInterface dbConnectorInterface = await GetByIdAsync(connectorInterface.Id);
                dbConnectorInterface.Description = connectorInterface.Description;
                dbConnectorInterface.Name = connectorInterface.Name;
                await _context.SaveChangesAsync();
            }
        }
    }
}
