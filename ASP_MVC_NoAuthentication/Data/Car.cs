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
        public int Id { set; get; }
        public String Brand { set; get; }
        public String Model { set; get; }
        public int MaximumDistance { set; get; }
        public virtual ICollection<Connector> Connectors { get; set; }

        public Car(int id, string brand, string model, int maximumDistance, ICollection<Connector> connectors)
		{
            this.Id = id;
            this.Brand = brand;
            this.Model = model;
            this.MaximumDistance = maximumDistance;
            this.Connectors = connectors;
		}

        public override string ToString()
        {
            return $"{Brand} {Model} ({MaximumDistance}, {ConnectorsToString()})";
        }

        public string ConnectorsToString()
        {
            string ret = "";
            if (Connectors.Count > 0)
            {
                foreach (Connector connector in Connectors)
                {
                    ret += connector.ToString() + ", ";
                }
            }
            else
            {
                ret = "Brak gniazd ładowania.";
            }
            return ret;
        }

    }
}
