using Microsoft.AspNetCore.Authorization;
using System.ComponentModel.DataAnnotations.Schema;

namespace ASP_MVC_NoAuthentication.Data
{
    public class Car
    {
        public Car()
        {
            this.Connectors = new HashSet<Connector>();
        }

        public Car(int id, string brand, string model, int maximumDistance, ICollection<Connector> connectors)
		{
            this.Id = id;
            this.Brand = brand;
            this.Model = model;
            this.MaximumDistance = maximumDistance;
            this.Connectors = connectors;
		}
        public int Id { set; get; }
        public String Brand { set; get; }
        public String Model { set; get; }
        public int MaximumDistance { set; get; }
        public ICollection<Connector> Connectors { get; set; }

        public override string ToString()
        {
            return $"{Brand} {Model} ({MaximumDistance}, {ConnectorsToString()})";
        }

        private string ConnectorsToString()
        {
            string ret = "";
            foreach(Connector connector in Connectors)
            {
                ret += connector.ToString() + ";";
            }
            return ret;
        }

    }
}
