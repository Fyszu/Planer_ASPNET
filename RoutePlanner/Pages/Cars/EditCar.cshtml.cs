using RoutePlanner.Data;
using RoutePlanner.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RoutePlanner.Pages
{
    [Authorize]
    public class EditCarModel : PageModel
    {
        private readonly ICarService carService;
        private readonly IConnectorInterfaceService connectorInterfacesService;
        private readonly IUserService userService;
        private readonly ILogger<EditCarModel> logger;
        private static int? carId;
        private User currentUser;

        public EditCarModel(ILogger<EditCarModel> logger, IUserService userService, ICarService carService, IConnectorInterfaceService connectorInterfacesService)
        {
            this.carService = carService;
            this.userService = userService;
            this.connectorInterfacesService = connectorInterfacesService;
            this.logger = logger;
        }

        public List<ConnectorInterface> ConnectorInterfaces { get; set; }
        public Car EditedCar { get; set; }

        public async Task OnGetAsync()
        {
            carId = null;
            await LoadData();
        }
        public async Task<IActionResult> OnPostAddCarAsync(string? returnUrl = null)
        {
            await LoadData();

            string brand = Request.Form["brand"];
            string model = Request.Form["carmodel"];
            int maximumDistance = (int.Parse(Request.Form["maximumdistance"]));
            string check;

            List<ConnectorInterface> carInterfaces = new();
            foreach (ConnectorInterface connectorInterface in ConnectorInterfaces)
            {
                string cb = "checkbox" + connectorInterface.Id;
                check = Request.Form[cb];
                if (check != null)
                    carInterfaces.Add(connectorInterface);
            }
            if (carInterfaces.Count > 0)
            {
                try
                {
                    await carService.UpdateCarAsync(new Car(EditedCar.Id, brand, model, maximumDistance, carInterfaces, currentUser));
                    logger.LogInformation($"Zaktualizowano samochód id: {EditedCar.Id} {brand} {model}, {maximumDistance}km dla u¿ytkownika {currentUser.UserName}.");
                    return Redirect(Url.Content("~/UserPanel"));
                }
                catch
                {
                    logger.LogError($"Problem podczas edycji samochodu {brand} {model}, {maximumDistance}km dla u¿ytkownika {currentUser.UserName}.");
                    throw new DatabaseException("Problem podczas edycji samochodu w bazie danych.");
                }
            }
            else
            {
                throw new NoDataInDatabaseException("B³¹d podczas edycji samochodu - lista interfejsów dla samochodu nie mo¿e byæ pusta.");
            }
        }

        private async Task LoadData()
        {
            ConnectorInterfaces = await connectorInterfacesService.GetAllConnectorInterfacesAsync();
            currentUser = await userService.GetUserByNameAsync(User.Identity.Name);

            if (ConnectorInterfaces == null || ConnectorInterfaces.Count == 0)
            {
                throw new NoDataInDatabaseException("B³¹d podczas dodawania samochodu: pobrana lista z³¹czy ³adowania jest pusta.");
            }
            if (currentUser == null)
            {
                throw new UserIsNullException("B³¹d podczas dodawania samochodu: nie znaleziono u¿ytkownika w bazie danych.");
            }

            if (carId == null)
            {
                try
                {
                    carId = int.Parse(Request.Query["carid"]);
                }
                catch (Exception ex)
                {
                    throw new Exception($"Z³y numer ID samochodu do edycji lub problem z odczytaniem ID.",new Exception($"{ex.Message}\n{ex.InnerException}"));
                }
            }

            EditedCar = await carService.GetCarByIdAsync(carId.Value);

            if (EditedCar == null && !(await carService.CheckIfCarBelongsToUserAsync(currentUser, EditedCar)))
            {
                throw new Exception("Samochód nie istnieje lub nie nale¿y do Ciebie.");
            }
        }
    }
}
