using AirlineReservationSystem.Core.Contracts;
using AirlineReservationSystem.Core.Models.AdminArea.Route;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AirlineReservationSystem.Areas.Admin.Controllers
{
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

        [HttpPost]
        public async Task<IActionResult> AddRoute(AddFlightRouteVM flightRouteVM)
        {
            await service.AddFlightRoute(flightRouteVM);

            //TODO: REDIRECT TO WHERE??

            return RedirectToAction("Home");

        }


    }
}
