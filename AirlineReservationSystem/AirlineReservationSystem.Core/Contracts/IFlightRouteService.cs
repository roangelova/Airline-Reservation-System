using AirlineReservationSystem.Core.Models.AdminArea.Route;

namespace AirlineReservationSystem.Core.Contracts
{
    public interface IFlightRouteService
    {
        Task<bool> AddFlightRoute(AddFlightRouteVM model);

        Task<IEnumerable<ListFlightRouteVM>> GetAllDepartureRoutes();
    }
}
