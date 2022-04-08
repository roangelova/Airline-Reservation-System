using AirlineReservationSystem.Core;
using AirlineReservationSystem.Core.Contracts;
using AirlineReservationSystem.Core.Models.AdminArea.Aircraft;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AirlineReservationSystem.Areas.Admin.Controllers
{
   // [Authorize(Roles = UserConstants.Role.FleetManagerRole)]
   // [Authorize(Roles = UserConstants.Role.AdministratorRole)]
    //TODO: remove Administrator Role after testing is done
    public class AircraftController : BaseController
    {
        private readonly IAircraftService service;

        public AircraftController(IAircraftService _service)
        {
            service = _service;
        }

        public IActionResult Home()
        {
            //TODO: show all available aircraft
            return View();
        } 
        
        [HttpGet]
        public IActionResult AddAircraft()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddAircraft(AddAircraftVM model)
        {


            //TODO: where should we redirect to once done?
            return View();
        }
    }
}
