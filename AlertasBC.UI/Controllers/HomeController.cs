using AlertasBC.Model;
using AlertasBC.Repository;
using AlertasBC.Model.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;
using System.Diagnostics;

namespace AlertasBC.UI.Controllers
{
    public class HomeController : Controller
    {
        CityRepository cities = new CityRepository();

        [Authorize]
        [HttpGet]
        public async Task<ActionResult> Index()
        {
            if (Session["User"] == null)
            {
                return RedirectToAction("LogOff", "Account");
            }
            var response = await cities.GetCities();
            ViewBag.CitiesList = response.Data;

            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Index(string[] SelectedCities, Notification notification)
        {
            if (Session["User"] == null)
            {
                return RedirectToAction("LogOff", "Account");
            }

            User user = Session["User"] as User;

            if (SelectedCities == null)
            {
                TempData["Message"] = "Seleccione al menos una ciudad.";
                var response = await cities.GetCities();
                ViewBag.CitiesList = response.Data;
                return View(notification);
            }

            try
            {
                List<Notification> notifications = new List<Notification>();
                foreach (string city in SelectedCities)
                {
                    notifications.Add(new Notification()
                    {
                        ID_CITY = city,
                        ID_DEPENDENCY =user.ID_DEPENDENCY,
                        NOTIFICATION_TEXT = notification.NOTIFICATION_TEXT,
                        CREATED_DATE = DateTime.Now
                    });
                }

                NotificationRepository notificationRespository = new NotificationRepository();
                RepositoryResponse<List<Notification>> response = await notificationRespository.AddNotifications(notifications);

                if (response.Success)
                {
                    TempData["Message"] = MvcHtmlString.Create("Notificación enviada exitosamente.");
                }
                else
                {
                    TempData["Message"] = "Se produjo un error, por favor intente nuevamente.";
                }
            }
            catch(Exception ex){
                TempData["Message"] = "Se produjo un error, por favor intente nuevamente.";
                Trace.TraceError(ex.Message);
            }

            return RedirectToAction("Index");
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        private List<City> GetCities()
        {
            List<City> citiesList = new List<City>();
            citiesList.Add(new City() { ID = "1", NAME = "Ensenada" });
            citiesList.Add(new City() { ID = "2", NAME = "Mexicali" });
            citiesList.Add(new City() { ID = "3", NAME = "Tijuana" });
            citiesList.Add(new City() { ID = "4", NAME = "Rosarito" });
            citiesList.Add(new City() { ID = "5", NAME = "Tecate" });

            return citiesList;
        }
    }
}
