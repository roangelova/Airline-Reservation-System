using AirlineReservationSystem.Core.Models.AdminArea.Route;

namespace AirlineReservationSystem.Core.Contracts
{
    public interface IFlightRouteService
    {
        Task<bool> AddFlightRoute(AddFlightRouteVM model);

        public Task<bool> CheckIfRouteInUse(string id);
        Task<IEnumerable<ListFlightRouteVM>> GetAllRoutes();
        Task<bool> RemoveRoute(string id);
    }
}
