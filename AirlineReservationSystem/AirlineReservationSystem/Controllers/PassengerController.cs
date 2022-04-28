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
           UserManager<ApplicationUser> _userManager) :base(_userManager)
        {
            passengerService = _passengerService;
            userService = _userService;
            userManager = _userManager;
        }


        /// <summary>
        /// Gets a static view with some flight information.
        /// </summary>
        public IActionResult FlightInfo()
        {
            return View();
        }

        public async Task<IActionResult> MyBookings()
        {
            var currentUserId = await GetUserIdAsync();
            var passengerId = await passengerService.GetPassengerId(currentUserId);

            if(passengerId == "")
            {
                return View("PassengerMustBeRegisteredError");
            };

            var userBookings = await passengerService.GetUserBookings(passengerId);

            return View(userBookings);
        }

        /// <summary>
        /// The method checks if the passenger data is already registered. If yes, customers are not allowed to register again and are
        /// redirected to a custom message view. Otherwise, users are redirected to the register passenger form.
        /// </summary>
        public async Task<IActionResult> EditPassengerData()
        {
            var currentUserId = await GetUserIdAsync();
            var PassengerId = passengerService.GetPassengerId(currentUserId);

            if (PassengerId != null)
            {
                return View("AlreadyRegistered");
            }

            return View();
        }

        public IActionResult Home()
        {
            return View();
        }


        /// <summary>
        /// Gets the viewmodel and passes it to the service, which registeres the passenger and returns the id. This is then passed 
        /// to the userService, in order to set the passengerId of the user.
        /// </summary>
        /// <returns>A view, depending on the outcome of the register passenger method</returns>
        [HttpPost]
        public async Task<IActionResult> EditPassengerData(EditPassengerDataVM model)
        {
            var currentUserId = await GetUserIdAsync();
            var (registeredSuccessfully, passengerId) = await passengerService.RegisterPassenger(model);

            if (passengerId != "")
            {
                await userService.SetPassengerId(currentUserId, passengerId);
            }

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
