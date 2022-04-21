using AirlineReservationSystem.Core.Models.User_Area;

namespace AirlineReservationSystem.Core.Contracts
{
    public interface IBaggageService
    {
        IEnumerable<AddBaggageVM> GetAvailableBaggageSizes();

        Task<bool> AddBaggageToBoooking (string BookingId, string PassengerID, AddBaggageVM model);

    }
}
