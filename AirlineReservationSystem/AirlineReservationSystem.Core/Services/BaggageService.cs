using AirlineReservationSystem.Core.Contracts;
using AirlineReservationSystem.Core.Models.User_Area;
using AirlineReservationSystem.Infrastructure.Models;

namespace AirlineReservationSystem.Core.Services
{
    public class BaggageService : IBaggageService
    {
        public IEnumerable<AddBaggageVM> GetAvailableBaggageSizes()
        {
            var availableSizes = Enum.GetValues<BaggageSize>()
                .Select(x => new AddBaggageVM() { Size = x.ToString() })
                .ToArray();
            
            return availableSizes;
        }
    }
}
