using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace ASP_MVC_NoAuthentication.Data
{
    public class Car
    {
        public Car()
        {
            this.Connectors = new HashSet<Connector>();
        }   
        public int Id { get; set; }
        public String Brand { set; get; }
        public String Model { set; get; }
        public int MaximumDistance { set; get; }
        public virtual ICollection<Connector> Connectors { get; set; }
        public User? User { get; set; }

        public Car(int id, string brand, string model, int maximumDistance, ICollection<Connector> connectors, User user)
		{
            this.Id = id;
            this.Brand = brand;
            this.Model = model;
            this.MaximumDistance = maximumDistance;
            this.Connectors = connectors;
            this.User = user;
		}

        public Car(string brand, string model, int maximumDistance, ICollection<Connector> connectors)
        {
            this.Brand = brand;
            this.Model = model;
            this.MaximumDistance = maximumDistance;
            this.Connectors = connectors;
        }

        public Car(string brand, string model, int maximumDistance, ICollection<Connector> connectors, User user)
        {
            this.Brand= brand;
            this.Model = model;
            this.MaximumDistance = maximumDistance;
            this.Connectors = connectors;
            this.User = user;
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
                ret = ret.Remove(ret.Length - 2);

            }
            else
            {
                ret = "Brak gniazd ładowania.";
            }
            return ret;
        }

    }
}
