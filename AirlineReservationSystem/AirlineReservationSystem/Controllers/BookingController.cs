using AirlineReservationSystem.Core.Contracts;
using AirlineReservationSystem.Infrastructure.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AirlineReservationSystem.Controllers
{
    public class BookingController : BaseController
    {
        private readonly IFlightService flightService;
        private readonly IBookingService bookingService;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IPassengerService passengerService;

        public BookingController(
            IFlightService _flightService,
            UserManager<ApplicationUser> _userManager,
            IPassengerService _passengerService,
            IBookingService _bookingService) : base(_userManager)
            
        {
            flightService = _flightService;
            userManager = _userManager;
            passengerService = _passengerService;
            bookingService = _bookingService;
        }  

        public async Task<IActionResult> Book()
        {
            var flights = await flightService.GetAllAvailableFlights();

            return View(flights);
        }

        /// <summary>
        /// Checks if the user is already registered as a passenger, as only then a user can book a flight. If yes, 
        /// the flight is booked and the user redirected to his bookings view. 
        /// </summary>

        [HttpPost]
        public async Task<IActionResult> Book(string id)
        {
            var currentUserId = await GetUserIdAsync();
            var PassengerId = await passengerService.GetPassengerId(currentUserId);

            if (PassengerId is null)
            {
                return View("PassengerMustBeRegisteredError");
            }

            bool bookedSuccessfully = await bookingService.BookPassengerFlight(id, PassengerId);

            if (bookedSuccessfully)
            {
                return RedirectToAction("MyBookings", "Passenger");
            }
            else
            {
                return View("CustomError");
            }
        }


        [HttpPost]
        public async Task<IActionResult> CancelBooking(string id)
        {
           bool success =  await bookingService.CancelBooking(id);

            if (success)
            {
                return RedirectToAction("MyBookings", "Passenger");
            }
            else
            {
                return View("CustomError");
            }

        }


        public async Task<IActionResult> GetArchivePastFlights()
        {
            var currentUserId = await GetUserIdAsync();
            var PassengerId = await passengerService.GetPassengerId(currentUserId);

            if(PassengerId == "")
            {
                return View("CustomError");
            };

            var pastUserFlights = await bookingService.GetPastUserFlights(PassengerId);

            return View(pastUserFlights);
        }

    }
}
