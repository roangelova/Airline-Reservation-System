using AirlineReservationSystem.Core.Contracts;
using AirlineReservationSystem.Core.Models.User_Area;
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

        public async Task<bool> BookPassengerFlight(string FlightId, string PassengerId)
        {
            bool bookedSuccessfully = false;

            var booking = new Booking()
            {
                FlightId = FlightId,
                PassengerId = PassengerId,
                BookingStatus = Infrastructure.Status.Scheduled
            };

            try
            {
                await repo.AddAsync<Booking>(booking);
                await repo.SaveChangesAsync();
                bookedSuccessfully = true;

                return bookedSuccessfully;
            }
            catch (Exception)
            {

            }

            return bookedSuccessfully;
        }

        public async Task<bool> CancelBooking(string BookingId)
        {
            bool removedSuccessfully = false;

            try
            {
                await repo.DeleteAsync<Booking>(BookingId);
                await repo.SaveChangesAsync();
                removedSuccessfully = true;
            }
            catch (Exception)
            {
                throw;
            }

            return removedSuccessfully;
        }


        public async Task<IEnumerable<PastUserFlightsVM>> GetPastUserFlights(string PassengerID)
        {
            var currentDate = DateTime.Now.Date;

           var UserFlights = await repo.All<Booking>()
                .Where(x => x.PassengerId == PassengerID)
                .Include(x => x.Flight)
                .Where(x=> x.Flight.FlightInformation.Date < currentDate)
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
