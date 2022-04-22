using AirlineReservationSystem.Core;
using AirlineReservationSystem.Core.Contracts;
using AirlineReservationSystem.Core.Models.AdminArea.Flight;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;
using System.Threading.Tasks;

namespace AirlineReservationSystem.Areas.Admin.Controllers
{
    [Authorize(Roles = UserConstants.Role.FlightManagerRole)]
    public class FlightController : BaseController
    {

        private readonly IAircraftService AircraftService;

        private readonly IFlightRouteService FlightRouteService;
        private readonly IFlightService flightService;

        public FlightController(
            IAircraftService _AircraftService,
            IFlightRouteService _FlightRouteService,
            IFlightService _flightService)
        {
            AircraftService = _AircraftService;
            FlightRouteService = _FlightRouteService;
            flightService = _flightService;
        }

        public IActionResult Home()
        {
            return View();
        }

        public async Task<IActionResult> AddFlight()
        {
            //TODO 
            //1. all airports for departure
            //2. airports - departure airport
            //3.all aircraft

           var availableAircrfats = await AircraftService.GetAllAircraft();
           var availableDepartures = await FlightRouteService.GetAllDepartureRoutes();
           
           
           
           ViewBag.AvailableAircraft = availableAircrfats
               .Select(a => new SelectListItem()
               {
                   Text = $"{a.Manufacturer} {a.AircraftModel}",
                   Value = a.Id
               })
               .ToList();
           
           ViewBag.AvailableDepartures = availableDepartures
               .Select(d => new SelectListItem()
               {
                   Text = d.City,
                   Value = d.Id
               })
               .ToList();

           

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddFlight(AddFlightVM model)
        {
            await flightService.AddFlight(model);

            return RedirectToAction("Home");

        }
    }
}
