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


        /// <summary>
        /// Adds a new flight route and makes sure the IATA code is always upper case as per standard
        /// </summary>
        public async Task<bool> AddFlightRoute(AddFlightRouteVM model)
        {
            bool addedSuccessfully = false;

            try
            {
                var flightRoute = new FlightRoute()
                {
                    City = model.City,
                    IATA = model.IATA.ToUpper(),
                };

                await repo.AddAsync(flightRoute);
                await repo.SaveChangesAsync();
                addedSuccessfully = true;
            }
            catch (Exception)
            {

                throw;
            }

            return addedSuccessfully;
        }

        /// <summary>
        /// Gets all available flight routes 
        /// </summary>
        public async Task<IEnumerable<ListFlightRouteVM>> GetAllRoutes()
        {
            return await repo.All<FlightRoute>()
                 .Select(f => new ListFlightRouteVM
                 {
                     City = f.City,
                     Id = f.RouteId,
                     IATA = f.IATA,
                 })
                 .ToListAsync();
        }

        /// <summary>
        /// Checks for the given flight route Id if it's currently in use (in a scheduled flight)
        /// and returns bool. 
        /// </summary>
        public async Task<bool> CheckIfRouteInUse(string id)
        {
            var FlightsInRoute = await repo.All<Flight>()
                .Where(x => x.FromId == id || x.ToId == id)
                .Where(x => x.FlightStatus == Infrastructure.Status.Scheduled)
                .ToListAsync();

            if (FlightsInRoute.Any())
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Removes a flight route by the given id
        /// </summary>
        public async Task<bool> RemoveRoute(string id)
        {
            bool removedSuccessfully = false;

            try
            {
                await repo.DeleteAsync<FlightRoute>(id);
                await repo.SaveChangesAsync();
                removedSuccessfully = true;
            }
            catch (Exception)
            {

                throw;
            }

            return removedSuccessfully;
        }
    }
}
