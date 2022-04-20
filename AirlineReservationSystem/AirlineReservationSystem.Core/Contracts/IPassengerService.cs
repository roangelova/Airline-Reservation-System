using AirlineReservationSystem.Core.Models.User_Area;

namespace AirlineReservationSystem.Core.Contracts
{
    public interface IPassengerService
    { 
        Task<bool> RegisterPassenger (EditPassengerDataVM model);
    }
}
