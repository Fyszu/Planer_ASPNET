using Microsoft.AspNetCore.Identity;

namespace ASP_MVC_NoAuthentication.Data
{
    public class User : IdentityUser
    {
        public User()
        {
            Cars = new HashSet<Car>();
        }
        public ICollection<Car> Cars { set; get; }
        public Boolean ShowOnlyMyCars { set; get; }
        public User(string id, bool showOnlyMyCars)
        {
            this.Id = id;
            this.ShowOnlyMyCars = showOnlyMyCars;
        }
    }
}
