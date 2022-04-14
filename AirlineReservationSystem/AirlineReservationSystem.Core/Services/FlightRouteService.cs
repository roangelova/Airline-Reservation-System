using AirlineReservationSystem.Core.Contracts;
using AirlineReservationSystem.Core.Models.AdminArea.Route;
using AirlineReservationSystem.Infrastructure.Models;
using AirlineReservationSystem.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

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

        public async Task<IEnumerable<ListFlightRouteVM>> GetAllDepartureRoutes()
        {
            return await repo.All<FlightRoute>()
                 .Select(f => new ListFlightRouteVM
                 {
                     City = f.City,
                     Id = f.RouteId
                 })
                 .ToListAsync();
        }
    }
}
