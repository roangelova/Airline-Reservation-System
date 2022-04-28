using AirlineReservationSystem.Core;
using AirlineReservationSystem.Core.Contracts;
using AirlineReservationSystem.Core.Models.AdminArea.Aircraft;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AirlineReservationSystem.Areas.Admin.Controllers
{
     [Authorize(Roles = UserConstants.Role.FleetManagerRole)]
   
    public class AircraftController : BaseController
    {
        private readonly IAircraftService service;

        public AircraftController(IAircraftService _service)
        {
            service = _service;
        }

        public async Task<IActionResult> Home()
        {
           var currentFleet = await service.GetAllAircraft();

            return View(currentFleet);
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
            var currentFleet = await service.GetAllAircraftForCancellation();

            return View(currentFleet);

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
