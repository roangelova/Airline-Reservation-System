using AirlineReservationSystem.Core.Contracts;
using AirlineReservationSystem.Core.Models.AdminArea.Flight;
using AirlineReservationSystem.Core.Models.User_Area;
using AirlineReservationSystem.Infrastructure;
using AirlineReservationSystem.Infrastructure.Models;
using AirlineReservationSystem.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace AirlineReservationSystem.Core.Services
{

    public class FlightService : IFlightService
    {

        private readonly IApplicatioDbRepository repo;

        public FlightService(
            IApplicatioDbRepository _repo)
        {
            repo = _repo;
        }

        public async Task<bool> AddFlight(AddFlightVM model)
        {
            bool addedSuccessfully = false;

            model.FlightInformation = model.FlightInformation.Replace('T', ' ');

            var flight = new Flight()
            {
                AircraftID = model.Aircraft,
                FlightInformation = DateTime.ParseExact(model.FlightInformation, "g", CultureInfo.InvariantCulture),
                FlightStatus = Infrastructure.Status.Scheduled,
                FromId = model.DepartureCity,
                ToId = model.ArrivalCity,
                StandardTicketPrice = decimal.Parse(model.StandardTicketPrice,CultureInfo.InvariantCulture),

            };
            

            await repo.AddAsync(flight);
            await repo.SaveChangesAsync();

            return addedSuccessfully;
        }


        public async Task<IEnumerable<AvailableFlightsVM>> GetAllAvailableFlights()
        {
            return await repo.All<Flight>()
                .Where(f => f.isAvailable == true && f.FlightStatus == Status.Scheduled)
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
