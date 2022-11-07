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

        public async Task Add(ConnectorInterface connectorInterface)
        {
            if (connectorInterface != null)
                await _context.Interfaces.AddAsync(connectorInterface);
            await _context.SaveChangesAsync();
        }

        public async Task<List<ConnectorInterface>> GetAll()
        {
            return await _context.Interfaces.ToListAsync();
        }

        public async Task<ConnectorInterface> GetById(long id)
        {
            return await _context.Interfaces.Where(cni => cni.Id.Equals(id)).SingleOrDefaultAsync();
        }

        public async Task Remove(ConnectorInterface connectorInterface)
        {
            if (connectorInterface != null)
                _context.Interfaces.Remove(connectorInterface);
            await _context.SaveChangesAsync();
        }

        // Removes all records from ConnectorInterfaces table
        public async Task RemoveAll()
        {
            await _context.Interfaces.ForEachAsync(cInterface => {
                if (cInterface != null)
                    _context.Interfaces.Remove(cInterface);
            });
            await _context.SaveChangesAsync();
        }

        public async Task Update(ConnectorInterface connectorInterface)
        {
            if (connectorInterface != null)
            {
                ConnectorInterface dbConnectorInterface = await GetById(connectorInterface.Id);
                dbConnectorInterface.Description = connectorInterface.Description;
                dbConnectorInterface.Name = connectorInterface.Name;
                await _context.SaveChangesAsync();
            }
        }
    }
}
