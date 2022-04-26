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
