using AirlineReservationSystem.Core.Models.AdminArea.Aircraft;

namespace AirlineReservationSystem.Core.Contracts
{
    public interface IAircraftService
    {
        Task<bool> AddAircraft(AddAircraftVM model);

        Task<IEnumerable<AddAircraftVM>> GetAllAircraft();
    }
}