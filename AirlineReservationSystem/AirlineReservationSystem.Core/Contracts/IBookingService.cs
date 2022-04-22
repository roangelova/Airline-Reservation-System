using AirlineReservationSystem.Core.Models.User_Area;

namespace AirlineReservationSystem.Core.Contracts
{
    public interface IBookingService
    {
        Task<IEnumerable<PastUserFlightsVM>> GetPastUserFlights(string PassengerID);

        Task<bool> BookPassengerFlight(string FlightId, string PassengerId);
        Task<bool> CancelBooking(string BookingId );
    }
}
