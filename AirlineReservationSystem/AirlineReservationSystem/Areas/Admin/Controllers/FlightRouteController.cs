using AirlineReservationSystem.Core;
using AirlineReservationSystem.Core.Contracts;
using AirlineReservationSystem.Core.Models.AdminArea.Route;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Threading.Tasks;

namespace AirlineReservationSystem.Areas.Admin.Controllers
{
    [Authorize(Roles = UserConstants.Role.FlightManagerRole)]
    public class FlightRouteController : BaseController
    {
        private readonly IFlightRouteService service;
        private readonly IMemoryCache cache;

        public FlightRouteController(IFlightRouteService _service,
            IMemoryCache _cache)
        {
            service = _service;
            cache = _cache;
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
            IEnumerable<ListFlightRouteVM> AvailableRoutes;

            if (!this.cache.TryGetValue("AvailableRoutes", out IEnumerable<ListFlightRouteVM> data))
            {
                data = await service.GetAllRoutes();

                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromSeconds(1000));

                this.cache.Set("AvailableRoutes", data, cacheEntryOptions);
            }

            AvailableRoutes = data;

            return View(AvailableRoutes);
        }

        [HttpPost]
        public async Task<IActionResult> AddRoute(AddFlightRouteVM flightRouteVM)
        {
            var result = await service.AddFlightRoute(flightRouteVM);

            if (result)
            {
                return RedirectToAction("Home");
            }
            else
            {
                return View("CustomError");
            }
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
