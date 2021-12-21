using System.ComponentModel.DataAnnotations;

namespace ASP_MVC_NoAuthentication.Data
{
    public class PersonalCar
    {
        public PersonalCar()
        {
            this.Connectors = new HashSet<Connector>();
        }
        public int Id { set; get; }
        public String Brand { set; get; }
        public String Model { set; get; }
        public int MaximumDistance { set; get; }
        public ICollection<Connector> Connectors { get; set; }

        [Required]
        public User User { set; get; }

        public override string ToString()
        {
            return $"{Brand} {Model} ({MaximumDistance}, {Connectors})";
        }
    }
}
