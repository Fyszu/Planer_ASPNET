using ASP_MVC_NoAuthentication.Data;

namespace ASP_MVC_NoAuthentication.Repositories
{
    public class ConnectorRepository : IConnectorRepository
    {
        private readonly MyDbContext _context;
        public ConnectorRepository(MyDbContext context)
        {
            _context = context;
        }



        public Connector GetById(int id)
        {
            return _context.Connectors.Find(id);
        }

        public void Add(Connector connector)
        {
            if(connector != null)
                _context.Add(connector);
            _context.SaveChanges();
        }

        public void Update(Connector connector)
        {
            if (connector != null)
            {
                Connector dbConnector = GetById(connector.Id);
                dbConnector.ChargingPoints = connector.ChargingPoints;
                dbConnector.Name = connector.Name;
            }
                
            _context.SaveChanges();
        }

        public void Remove(Connector connector)
        {
            if(connector != null)
                _context.Remove(connector);
            _context.SaveChanges();
        }

        public List<Connector> GetAll()
        {
            return _context.Connectors.ToList();
        }
    }
}
