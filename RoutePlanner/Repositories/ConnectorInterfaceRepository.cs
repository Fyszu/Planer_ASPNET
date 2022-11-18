using RoutePlanner.Data;
using Microsoft.EntityFrameworkCore;

namespace RoutePlanner.Repositories
{
    public class ConnectorInterfaceRepository : IConnectorInterfaceRepository
    {
        private readonly MyDbContext context;
        public ConnectorInterfaceRepository(MyDbContext context)
        {
            this.context = context;
        }

        public async Task AddAsync(ConnectorInterface connectorInterface)
        {
            if (connectorInterface != null)
                await context.ConnectorInterfaces.AddAsync(connectorInterface);
            await context.SaveChangesAsync();
        }
        public List<ConnectorInterface> GetAll()
        {
            return context.ConnectorInterfaces.ToList();
        }
        public async Task<List<ConnectorInterface>> GetAllAsync()
        {
            return await context.ConnectorInterfaces.ToListAsync();
        }

        public async Task<ConnectorInterface> GetByIdAsync(long id)
        {
            return await context.ConnectorInterfaces.Where(cni => cni.Id.Equals(id)).SingleOrDefaultAsync();
        }

        public async Task RemoveAsync(ConnectorInterface connectorInterface)
        {
            if (connectorInterface != null)
                context.ConnectorInterfaces.Remove(connectorInterface);
            await context.SaveChangesAsync();
        }

        // Removes all records from ConnectorInterfaces table
        public async Task RemoveAllAsync()
        {
            await context.ConnectorInterfaces.ForEachAsync(cInterface => {
                if (cInterface != null)
                    context.ConnectorInterfaces.Remove(cInterface);
            });
            await context.SaveChangesAsync();
        }

        public async Task UpdateAsync(ConnectorInterface connectorInterface)
        {
            if (connectorInterface != null)
            {
                ConnectorInterface dbConnectorInterface = await GetByIdAsync(connectorInterface.Id);
                dbConnectorInterface.Description = connectorInterface.Description;
                dbConnectorInterface.Name = connectorInterface.Name;
                await context.SaveChangesAsync();
            }
        }
    }
}
