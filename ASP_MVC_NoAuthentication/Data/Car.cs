using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ASP_MVC_NoAuthentication.Data
{
    public class Car
    {
        public Car()
        {
            ConnectorInterfaces = new HashSet<ConnectorInterface>();
        }
        public int Id { get; set; }
        [Required]
        public string Brand { set; get; }
        [Required]
        public string Model { set; get; }
        [Required]
        public int MaximumDistance { set; get; }
        public ICollection<ConnectorInterface> ConnectorInterfaces { get; set; }
        public User? User { get; set; }
        public Car(int id, string brand, string model, int maximumDistance, ICollection<ConnectorInterface> interfaces, User user)
		{
            this.Id = id;
            this.Brand = brand;
            this.Model = model;
            this.MaximumDistance = maximumDistance;
            this.ConnectorInterfaces = interfaces;
            this.User = user;
		}

        public Car(string brand, string model, int maximumDistance, ICollection<ConnectorInterface> interfaces)
        {
            this.Brand = brand;
            this.Model = model;
            this.MaximumDistance = maximumDistance;
            this.ConnectorInterfaces = interfaces;
        }

        public Car(string brand, string model, int maximumDistance, ICollection<ConnectorInterface> interfaces, User user)
        {
            this.Brand= brand;
            this.Model = model;
            this.MaximumDistance = maximumDistance;
            this.ConnectorInterfaces = interfaces;
            this.User = user;
        }

        public override string ToString()
        {
            return $"{Brand} {Model} ({MaximumDistance}, {CarConnectorInterfacesToString()})";
        }

        public string CarConnectorInterfacesToString()
        {
            string ret = "";
            if (ConnectorInterfaces.Count > 0)
            {
                foreach (ConnectorInterface cnInterface in ConnectorInterfaces)
                {
                    ret += cnInterface.ToString() + ", ";
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
