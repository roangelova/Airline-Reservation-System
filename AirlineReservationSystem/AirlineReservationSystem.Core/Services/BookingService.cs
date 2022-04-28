using AirlineReservationSystem.Core.Contracts;
using AirlineReservationSystem.Core.Models.User_Area;
using AirlineReservationSystem.Infrastructure;
using AirlineReservationSystem.Infrastructure.Models;
using AirlineReservationSystem.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace AirlineReservationSystem.Core.Services
{
    public class BookingService : IBookingService
    {
        private readonly IApplicatioDbRepository repo;

        public BookingService(IApplicatioDbRepository _repo)
        {
            repo = _repo;
        }

        /// <summary>
        /// Created a new booking for the given passenger and Flight Id. By default status is scheduled
        /// </summary>
        public async Task<bool> BookPassengerFlight(string FlightId, string PassengerId)
        {
            bool bookedSuccessfully = false;

            try
            {
                var booking = new Booking()
                {
                    FlightId = FlightId,
                    PassengerId = PassengerId,
                    BookingStatus = Infrastructure.Status.Scheduled
                };

                await repo.AddAsync<Booking>(booking);
                await repo.SaveChangesAsync();
                bookedSuccessfully = true;
            }
            catch (Exception)
            {
                throw;
            }

            return bookedSuccessfully;
        }

        /// <summary>
        /// Cancels the user booking with the given Id
        /// </summary>
        public async Task<bool> CancelBooking(string BookingId)
        {
            bool canceledSuccessfully = false;

            try
            {
                var BookingToCancel = await repo.GetByIdAsync<Booking>(BookingId);
                BookingToCancel.BookingStatus = Status.Canceled;
                await repo.SaveChangesAsync();
                canceledSuccessfully = true;
            }
            catch (Exception)
            {
                throw;
            }

            return canceledSuccessfully;
        }

        /// <summary>
        /// Gets an archive of the user flights before current date. 
        /// </summary>
        public async Task<IEnumerable<PastUserFlightsVM>> GetPastUserFlights(string PassengerID)
        {
            var currentDate = DateTime.Now.Date;

            var UserFlights = await repo.All<Booking>()
                 .Where(x => x.PassengerId == PassengerID)
                 .Include(x => x.Flight)
                 .Where(x => x.Flight.FlightInformation.Date < currentDate)
                 .Select(x =>
                 new PastUserFlightsVM
                 {
                     DepartureDestination = x.Flight.To.City,
                     ArrivalDestination = x.Flight.From.City,
                     DateAndTime = x.Flight.FlightInformation.ToString(),
                     BookingId = x.BookingNumber
                 })
                 .ToListAsync();

            return UserFlights;
        }


    }
}
