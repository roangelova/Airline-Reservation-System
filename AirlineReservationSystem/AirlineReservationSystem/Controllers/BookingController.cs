using Microsoft.AspNetCore.Mvc;

namespace AirlineReservationSystem.Controllers
{
    public class BookingController : BaseController
    {
        public IActionResult Book()
        {
            return View();
        }
    }
}
