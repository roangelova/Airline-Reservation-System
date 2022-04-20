using AirlineReservationSystem.Core.Models.User_Area;

namespace AirlineReservationSystem.Core.Contracts
{
    public interface IBookingService
    {
        Task<IEnumerable<AvailableFlightsVM>> GetAllAvailableFlights();

        Task<bool> BookPassengerFlight(string FlightId, string PassengerId);
    }
}
