using AirlineReservationSystem.Core.Contracts;
using AirlineReservationSystem.Core.Models.AdminArea.Route;
using AirlineReservationSystem.Infrastructure.Models;
using AirlineReservationSystem.Infrastructure.Repositories;

namespace AirlineReservationSystem.Core.Services
{
    public class FlightRouteService : IFlightRouteService
    {
        private readonly IApplicatioDbRepository repo;

        public FlightRouteService(IApplicatioDbRepository _repo)
        {
            repo = _repo;
        }



        public async Task<bool> AddFlightRoute(AddFlightRouteVM model)
        {
            bool addedSuccessfully = false;

            var flightRoute = new FlightRoute()
            {
                City = model.City,
                IATA = model.IATA.ToUpper(),
            };

            await repo.AddAsync(flightRoute);
            await repo.SaveChangesAsync();

            return addedSuccessfully;

        }
    }
}
