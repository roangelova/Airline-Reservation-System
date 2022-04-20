using AirlineReservationSystem.Core.Contracts;
using AirlineReservationSystem.Core.Models.User_Area;
using Microsoft.AspNetCore.Mvc;

namespace AirlineReservationSystem.Controllers
{
    public class PassengerController : BaseController
    {
        private readonly IPassengerService passengerService;

        public PassengerController(IPassengerService _passengerService)
        {
            passengerService = _passengerService;
        }


        public IActionResult FlightInfo()
        {
            return View();
        }

        public IActionResult EditPassengerData()
        {
            return View();
        }

        public IActionResult Home()
        {
            return View();
        }

        [HttpPost]

        public async Task<IActionResult> EditPassengerData(EditPassengerDataVM model)
        {
            var registeredSuccessfully = await passengerService.RegisterPassenger(model);

            if (registeredSuccessfully)
            {
                return View("RegisteredSuccessfully");
            }
            else
            {

                return View("CustomError");
            }

        }
    }
}
