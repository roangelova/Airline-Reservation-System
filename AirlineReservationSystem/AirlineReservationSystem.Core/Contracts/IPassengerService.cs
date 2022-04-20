using AirlineReservationSystem.Core.Models.User_Area;

namespace AirlineReservationSystem.Core.Contracts
{
    public interface IPassengerService
    {
        Task<(bool result, string passengerId)> RegisterPassenger (EditPassengerDataVM model);

        Task<IEnumerable<MyBookingsVM>> GetUserBookings(string id);

        Task<string> GetPassengerId(string UserId);
    }
}
