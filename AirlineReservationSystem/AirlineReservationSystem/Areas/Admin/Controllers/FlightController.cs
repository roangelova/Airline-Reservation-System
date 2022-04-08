using Microsoft.AspNetCore.Mvc;

namespace AirlineReservationSystem.Areas.Admin.Controllers
{
    public class FlightController : BaseController
    {
        public IActionResult Home()
        {
            return View();
        }
    }
}
