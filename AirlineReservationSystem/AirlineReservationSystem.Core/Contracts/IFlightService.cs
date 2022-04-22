using AirlineReservationSystem.Core.Models.AdminArea.Flight;
using AirlineReservationSystem.Core.Models.User_Area;

namespace AirlineReservationSystem.Core.Contracts
{
    public interface IFlightService
    {
        
        Task<bool> AddFlight(AddFlightVM model);

        Task<IEnumerable<AvailableFlightsVM>> GetAllAvailableFlights();

    }
}
