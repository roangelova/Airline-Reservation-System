using Microsoft.AspNetCore.Mvc;

namespace AirlineReservationSystem.Controllers
{
    public class PassengerController : BaseController
    {
        public IActionResult Home()
        {
            return View();
        }
    }
}
