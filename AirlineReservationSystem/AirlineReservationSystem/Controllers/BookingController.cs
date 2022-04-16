using AirlineReservationSystem.Core.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace AirlineReservationSystem.Controllers
{
    public class BookingController : BaseController
    {
        private readonly IBookingService bookingService;

        public BookingController(IBookingService _bookingService)
        {
            bookingService = _bookingService;
        }  

        public async Task<IActionResult> Book()
        {
            var flights = await bookingService.GetAllAvailableFlights();

            return View(flights);
        }
    }
}
