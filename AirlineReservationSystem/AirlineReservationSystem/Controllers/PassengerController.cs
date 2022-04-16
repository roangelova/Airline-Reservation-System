using Microsoft.AspNetCore.Mvc;

namespace AirlineReservationSystem.Controllers
{
    public class PassengerController : BaseController
    {
        public IActionResult FlightInfo()
        {
            return View();
        }

        public IActionResult EditPassengerData()
        {
            return View();
        }

        public IActionResult Home()
        {
            return View();
        }
    }
}
