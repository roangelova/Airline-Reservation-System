using AirlineReservationSystem.Core.Contracts;
using AirlineReservationSystem.Infrastructure.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AirlineReservationSystem.Controllers
{
    public class BookingController : BaseController
    {
        private readonly IBookingService bookingService;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IPassengerService passengerService;

        public BookingController(
            IBookingService _bookingService,
            UserManager<ApplicationUser> _userManager,
            IPassengerService _passengerService)
        {
            bookingService = _bookingService;
            userManager = _userManager;
            passengerService = _passengerService;
        }  

        public async Task<IActionResult> Book()
        {
            var flights = await bookingService.GetAllAvailableFlights();

            return View(flights);
        }


        [HttpPost]
        public async Task<IActionResult> Book(string FlightId)
        {
            //GET USER AND PASSENGER ID
            var user = await userManager.GetUserAsync(this.User);
            var currentUserId = await userManager.GetUserIdAsync(user);
            var PassengerId = await passengerService.GetPassengerId(currentUserId);

            if (PassengerId is null)
            {
                return View("PassengerMustBeRegisteredError");
            }

            bool bookedSuccessfully = await bookingService.BookPassengerFlight(FlightId, PassengerId);

            if (bookedSuccessfully)
            {
                return RedirectToAction("MyBookings", "Passenger");
            }
            else
            {
                return View("CustomError");
            }
        }
    }
}
