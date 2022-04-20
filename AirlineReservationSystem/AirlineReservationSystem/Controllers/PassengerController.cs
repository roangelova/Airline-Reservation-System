using AirlineReservationSystem.Core.Contracts;
using AirlineReservationSystem.Core.Models.User_Area;
using AirlineReservationSystem.Infrastructure.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AirlineReservationSystem.Controllers
{
    public class PassengerController : BaseController
    {
        private readonly IPassengerService passengerService;
        private readonly IUserService userService;
        private readonly UserManager<ApplicationUser> userManager;

        public PassengerController(
            IPassengerService _passengerService, IUserService _userService,
           UserManager<ApplicationUser> _userManager)
        {
            passengerService = _passengerService;
            userService = _userService;
            userManager = _userManager;
        }


        public IActionResult FlightInfo()
        {
            return View();
        }

        public async Task<IActionResult> MyBookings()
        {
            var user = await userManager.GetUserAsync(this.User);
            var currentUserId = await userManager.GetUserIdAsync(user);

            var userBookings = await passengerService.GetUserBookings(currentUserId);

            return View(userBookings);
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
            var (registeredSuccessfully, passengerId) = await passengerService.RegisterPassenger(model);

            var user = await userManager.GetUserAsync(this.User);
            var currentUserId = await userManager.GetUserIdAsync(user);

            await userService.SetPassengerId(currentUserId, passengerId);

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
