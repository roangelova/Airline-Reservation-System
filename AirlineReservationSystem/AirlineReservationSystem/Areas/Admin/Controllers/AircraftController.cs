using AirlineReservationSystem.Core;
using AirlineReservationSystem.Core.Contracts;
using AirlineReservationSystem.Core.Models.AdminArea.Aircraft;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Threading.Tasks;

namespace AirlineReservationSystem.Areas.Admin.Controllers
{
     [Authorize(Roles = UserConstants.Role.FleetManagerRole)]
   
    public class AircraftController : BaseController
    {
        private readonly IAircraftService service;
        private readonly IMemoryCache cache;

        public AircraftController(
            IAircraftService _service,
            IMemoryCache _cache)
        {
            service = _service;
            cache = _cache;
        }

        public async Task<IActionResult> Home()
        {
            IEnumerable<GetAircraftDataVM> CurrentFleet;

            if (!this.cache.TryGetValue("CurrentFleet", out IEnumerable<GetAircraftDataVM> data))
            {
                data = await service.GetAllAircraft();

                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromSeconds(1000));

                this.cache.Set("CurrentFleet", data, cacheEntryOptions);
            }

            CurrentFleet = data;

            return View(CurrentFleet);
        }

        [HttpGet]
        public IActionResult AddAircraft()
        {
            return View();
        }

        /// <summary>
        /// Gets a table of Type or aircraft + Id 
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> RemoveAircraft()
        {
            IEnumerable<GetAircraftDataVM> CurrentFleetForRemoval;

            if (!this.cache.TryGetValue("CurrentFleetForRemoval", out IEnumerable<GetAircraftDataVM> data))
            {
                data = await service.GetAllAircraftForCancellation();


                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromSeconds(1000));

                this.cache.Set("CurrentFleetForRemoval", data, cacheEntryOptions);
            }

            CurrentFleetForRemoval = data;

            return View(CurrentFleetForRemoval);

        }


        /// <summary>
        /// Removes the aircraft by the given id. If the aircraft is usded in an existing (scheduled) flight,
        /// the operation won't be allowed.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> RemoveAircraft(string id)
        {
            bool IsInUse = await service.CheckIfInUse(id);

            if (IsInUse)
            {
                return View("CantPerformThisAction");
            }

            bool removedSuccessfully = await service.RemoveAircraft(id);

            if (removedSuccessfully)
            {
                return View("Success");
            }
            else
            {
                return View("CustomError");
            }

        }

        [HttpPost]
        public async Task<IActionResult> AddAircraft(AddAircraftVM addAircraftVM)
        {
            var addedSuccessfully = await service.AddAircraft(addAircraftVM);

            if (addedSuccessfully)
            {
                return RedirectToAction("Home");
            }
            else
            {
                return View("CustomError");
            }

        }
    }
}
