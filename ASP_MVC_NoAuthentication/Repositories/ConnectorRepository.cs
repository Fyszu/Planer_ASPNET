using ASP_MVC_NoAuthentication.Data;

namespace ASP_MVC_NoAuthentication.Repositories
{
    public class ConnectorRepository
    {
        private readonly MyDbContext _context;
        public ConnectorRepository(MyDbContext context)
        {
            _context = context;
        }

        public Connector getConnectorById(int id)
        {
            List<Connector> connectors = _context.Connectors.ToList();
            foreach (Connector connector in connectors)
            {
                if (connector.Id == id)
                    return connector;
            }
            return null;
        }

        public List<Connector> GetAllConnectors()
        {
            return _context.Connectors.ToList();
        }
    }
}
