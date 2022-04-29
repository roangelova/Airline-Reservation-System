using AirlineReservationSystem.Core;
using AirlineReservationSystem.Core.Contracts;
using AirlineReservationSystem.Core.Models.AdminArea.Flight;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Caching.Memory;
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
        private readonly IMemoryCache cache;

        public FlightController(
            IAircraftService _AircraftService,
            IFlightRouteService _FlightRouteService,
            IFlightService _flightService,
            IMemoryCache _cache)
        {
            AircraftService = _AircraftService;
            FlightRouteService = _FlightRouteService;
            flightService = _flightService;
            cache = _cache;
        }

        public IActionResult Home()
        {
            return View();
        }

        public async Task<IActionResult> AddFlight()
        {
            var availableAircrfats = await AircraftService.GetAllAircraft();
            var availableDepartures = await FlightRouteService.GetAllRoutes();

            ViewBag.AvailableAircraft = availableAircrfats
                .Select(a => new SelectListItem()
                {
                    Text = a.AircraftMakeAndModel,
                    Value = a.AircraftId
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
            if (model.ArrivalCity== model.DepartureCity)
            {
                return View("NotAllowed");
            }

            var addedSuccessfuly = await flightService.AddFlight(model);

            if (addedSuccessfuly)
            {
                return RedirectToAction("Home");
            }
            else
            {
                return View("CustomError");
            }

        }

        public async Task<IActionResult> CancelFlight()
        {
            IEnumerable<FlightsForCancellationVM> FlightsForCancellation;

            if (!this.cache.TryGetValue("FlightsForCancellation", out IEnumerable<FlightsForCancellationVM> data))
            {
                data = await flightService.GetFlightsForCancellation();

                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromSeconds(1000));

                this.cache.Set("FlightsForCancellation", data, cacheEntryOptions);
            }

            FlightsForCancellation = data;

            return View(FlightsForCancellation);
        }

        [HttpPost]
        public async Task<IActionResult> CancelFlight(string id)
        {
            var result = await flightService.CancelFlight(id);

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
