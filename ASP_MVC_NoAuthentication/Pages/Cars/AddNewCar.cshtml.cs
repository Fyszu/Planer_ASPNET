using ASP_MVC_NoAuthentication.Data;
using ASP_MVC_NoAuthentication.Repositories;
using ASP_MVC_NoAuthentication.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ASP_MVC_NoAuthentication.Pages
{
    [Authorize]
    public class AddNewCarModel : PageModel
    {
        private readonly ICarService carService;
        private readonly IConnectorInterfaceService connectorInterfaceService;
        private readonly ILogger<AddNewCarModel> logger;
        private readonly IUserService userService;
        private User currentUser;

        public AddNewCarModel(ILogger<AddNewCarModel> logger, IUserService userService, ICarService carService, IConnectorInterfaceService connectorInterfaceService)
        {
            this.userService = userService;
            this.carService = carService;
            this.connectorInterfaceService = connectorInterfaceService;
            this.logger = logger;
        }

        public List<ConnectorInterface> ConnectorInterfaces { get; set; }

        public async Task OnGetAsync()
        {
            await LoadData();
        }

        public async Task<IActionResult> OnPostAddCarAsync()
        {
            await LoadData();

            if (currentUser.Cars.Count > 30)
            {
                logger.LogInformation($"U¿ytkownik {currentUser.UserName} przekroczy³ limit samochodów na koncie.");
                throw new Exception("Przekroczono limit samochodów na koncie. Maksymalna liczba samochodów dla u¿ytkownika to 30.");
            }

            string brand = Request.Form["brand"];
            string model = Request.Form["carmodel"];
            int maximumDistance = int.Parse(Request.Form["maximumdistance"]);
            string check;

            List<ConnectorInterface> carInterfaces = new();
            foreach(ConnectorInterface connectorInterface in ConnectorInterfaces)
            {
                string cb = "checkbox" + connectorInterface.Id;
                check = Request.Form[cb];
                if(check != null)
                    carInterfaces.Add(connectorInterface);
            }

            if (carInterfaces.Count > 0)
            {
                try
                {
                    await carService.AddNewCarAsync(new Car(brand, model, maximumDistance, carInterfaces, currentUser));
                    logger.LogInformation($"Dodano samochód {brand} {model}, {maximumDistance}km dla u¿ytkownika {currentUser.UserName}.");
                    return Redirect(Url.Content("~/UserPanel"));
                }
                catch
                {
                    logger.LogError($"Problem podczas dodawania samochodu {brand} {model}, {maximumDistance}km do bazy danych dla u¿ytkownika {currentUser.UserName}.");
                    throw new DatabaseException("Problem podczas dodawania samochodu do bazy danych.");
                }
            }
            else
            {
                logger.LogError($"B³¹d podczas dodawania samochodu - lista interfejsów by³a pusta dla u¿ytkownika {currentUser.UserName}.");
                throw new NoDataInDatabaseException("B³¹d podczas dodawania samochodu - lista interfejsów dla samochodu nie mo¿e byæ pusta.");
            }
        }

        private async Task LoadData()
        {
            ConnectorInterfaces = await connectorInterfaceService.GetAllConnectorInterfacesAsync();
            currentUser = await userService.GetUserByNameAsync(User.Identity.Name);
            if (ConnectorInterfaces == null || ConnectorInterfaces.Count == 0)
            {
                logger.LogError("B³¹d podczas dodawania samochodu: pobrana lista z³¹czy ³adowania jest pusta.");
                throw new NoDataInDatabaseException("B³¹d podczas dodawania samochodu: pobrana lista z³¹czy ³adowania jest pusta.");
            }
            if (currentUser == null)
            {
                logger.LogError($"B³¹d podczas dodawania samochodu: nie znaleziono u¿ytkownika {User.Identity.Name} w bazie danych.");
                throw new UserIsNullException("B³¹d podczas dodawania samochodu: nie znaleziono u¿ytkownika w bazie danych.");
            }
        }
    }
}
