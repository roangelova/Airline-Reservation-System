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

            //WE NEED PASSENGER ID INSTEAD OF USER ID
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

        public async Task<IEnumerable<AvailableFlightsVM>> GetAllAvailableFlights()
        {
            return await repo.All<Flight>()
                .Select(x =>
                new AvailableFlightsVM
                {
                    DepartureDestination = x.To.City,
                    ArrivalDestination = x.From.City,
                    DateAndTime = x.FlightInformation.ToString(),
                    Price = x.StandardTicketPrice.ToString(),
                    FlightId = x.FlightId
                })
                .ToListAsync();
        }
    }
}
