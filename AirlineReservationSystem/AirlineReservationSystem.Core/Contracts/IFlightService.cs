using AirlineReservationSystem.Core.Models.AdminArea.Flight;
using AirlineReservationSystem.Core.Models.User_Area;

namespace AirlineReservationSystem.Core.Contracts
{
    public interface IFlightService
    {
        
        Task<bool> AddFlight(AddFlightVM model);
        Task<bool> CancelFlight(string FlightId);
        Task<IEnumerable<AvailableFlightsVM>> GetAllAvailableFlights();
        Task<IEnumerable<FlightsForCancellationVM>> GetFlightsForCancellation();

    }
}
