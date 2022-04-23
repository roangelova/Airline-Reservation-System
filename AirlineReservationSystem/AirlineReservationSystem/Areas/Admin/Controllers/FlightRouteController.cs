using AirlineReservationSystem.Core;
using AirlineReservationSystem.Core.Contracts;
using AirlineReservationSystem.Core.Models.AdminArea.Route;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AirlineReservationSystem.Areas.Admin.Controllers
{
    [Authorize(Roles = UserConstants.Role.FlightManagerRole)]
    public class FlightRouteController : BaseController
    {
        private readonly IFlightRouteService service;

        public FlightRouteController(IFlightRouteService _service)
        {
            service = _service;
        }
        public IActionResult Home()
        {
            return View();
        }

        [HttpGet]
        public IActionResult AddRoute()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> RemoveRoute()
        {
            var routes = await service.GetAllRoutes();

            return View(routes);
        }

        [HttpPost]
        public async Task<IActionResult> AddRoute(AddFlightRouteVM flightRouteVM)
        {
            await service.AddFlightRoute(flightRouteVM);

            return RedirectToAction("Home");

        }

        [HttpPost]
        public async Task<IActionResult> RemoveRoute(string id)
        {
            var IsInUse = await service.CheckIfRouteInUse(id);
            if (IsInUse)
            {
                return View("CantPerformThisAction");
            }
            else
            {
                var result = await service.RemoveRoute(id);

                if (result)
                {
                    return View("Success");
                }
                else
                {
                    return View("CustomError");
                }
            }
        }

    }
}
