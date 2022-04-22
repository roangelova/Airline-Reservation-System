using AirlineReservationSystem.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AirlineReservationSystem.Areas.Admin.Controllers
{
    [Authorize(Roles ="Administrator, Fleet Manager, Flight Manager")]
    public class HomeController : BaseController
    {
        public IActionResult Index()
        {
            var User = this.User;
            return View();
        }
    }
}
