using AirlineReservationSystem.Core.Contracts;
using AirlineReservationSystem.Core.Models.AdminArea.Flight;
using AirlineReservationSystem.Infrastructure.Models;
using AirlineReservationSystem.Infrastructure.Repositories;
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
    }
}
