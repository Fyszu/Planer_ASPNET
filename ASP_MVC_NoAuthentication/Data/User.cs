using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace ASP_MVC_NoAuthentication.Data
{
    public class User : IdentityUser
    {
        public User()
        {
            Cars = new HashSet<Car>();
        }
        public ICollection<Car> Cars { set; get; }
        [Required]
        public Boolean ShowOnlyMyCars { set; get; }
        public User(string id)
        {
            this.Id = id;
            this.ShowOnlyMyCars = false;
            this.Cars = new HashSet<Car>();
        }
    }
}
