using AirlineReservationSystem.Core.Models.AdminArea.Aircraft;

namespace AirlineReservationSystem.Core.Contracts
{
    public interface IAircraftService
    {
        Task<bool> AddAircraft(AddAircraftVM model);
        Task<bool> RemoveAircraft(string AircraftId);
        Task<bool> CheckIfInUse(string AircraftId);
        Task<IEnumerable<GetAircraftDataVM>> GetAllAircraft();
        public Task<IEnumerable<GetAircraftDataVM>> GetAllAircraftForCancellation();
    }
}