using AirlineReservationSystem.Core.Contracts;
using AirlineReservationSystem.Core.Models.User_Area;
using AirlineReservationSystem.Infrastructure.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace AirlineReservationSystem.Controllers
{
    public class BookingController : BaseController
    {
        private readonly IFlightService flightService;
        private readonly IBookingService bookingService;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IPassengerService passengerService;
        private readonly IMemoryCache cache;

        public BookingController(
            IFlightService _flightService,
            UserManager<ApplicationUser> _userManager,
            IPassengerService _passengerService,
            IBookingService _bookingService,
            IMemoryCache _cache) : base(_userManager, _cache)

        {
            flightService = _flightService;
            userManager = _userManager;
            passengerService = _passengerService;
            bookingService = _bookingService;
            cache = _cache;
        }

        public async Task<IActionResult> Book()
        {
            IEnumerable<AvailableFlightsVM> AvailableFlights;

            if (!this.cache.TryGetValue("AvailableFlights", out IEnumerable<AvailableFlightsVM> flights))
            {
                flights = await flightService.GetAllAvailableFlights();

                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromSeconds(1000));

                this.cache.Set("AvailableFlights", flights, cacheEntryOptions);
            }

            AvailableFlights = flights;

            return View(AvailableFlights);
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

            if (PassengerId == "")
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
            bool success = await bookingService.CancelBooking(id);

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

            if (PassengerId == "")
            {
                return View("CustomError");
            };

            IEnumerable<PastUserFlightsVM> PastUserFlights;

            if (!this.cache.TryGetValue("pastUserFlights", out IEnumerable<PastUserFlightsVM> flights))
            {
                flights = await bookingService.GetPastUserFlights(PassengerId);

                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromSeconds(1000));

                this.cache.Set("pastUserFlights", flights, cacheEntryOptions);
            }

            PastUserFlights = flights;

            return View(PastUserFlights);
        }

    }
}
