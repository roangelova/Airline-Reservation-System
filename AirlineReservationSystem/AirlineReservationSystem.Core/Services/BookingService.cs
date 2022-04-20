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
                })
                .ToListAsync();
        }
    }
}
