using AirlineReservationSystem.Core.Models.AdminArea.Flight;

namespace AirlineReservationSystem.Core.Contracts
{
    public interface IFlightService
    {
        Task<bool> AddFlight(AddFlightVM model);
    }
}
