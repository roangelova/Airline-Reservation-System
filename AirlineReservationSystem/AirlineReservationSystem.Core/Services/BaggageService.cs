using AirlineReservationSystem.Core.Contracts;
using AirlineReservationSystem.Core.Models.User_Area;
using AirlineReservationSystem.Infrastructure.Models;
using AirlineReservationSystem.Infrastructure.Repositories;

namespace AirlineReservationSystem.Core.Services
{
    public class BaggageService : IBaggageService
    {

        private readonly IApplicatioDbRepository repo;

        public BaggageService(IApplicatioDbRepository _repo)
        {
            repo = _repo;
        }
        public async Task<bool> AddBaggageToBoooking(string BookingId, string PassengerId, AddBaggageVM model)
        {
            var Size = Enum.GetValues<BaggageSize>().Where(x => x.Equals( model.Size)).FirstOrDefault();
            bool addedSuccessfully = false;

            var baggage = new Baggage()
            {
                Size = Size,
                IsReportedLost = false,
                BookingId = BookingId,
                PassengerId = PassengerId
            };

            try
            {
               await repo.AddAsync<Baggage>(baggage);
               await repo.SaveChangesAsync();
                addedSuccessfully = true;
            }
            catch (Exception)
            {

            }

            return addedSuccessfully;
        }

        public IEnumerable<AddBaggageVM> GetAvailableBaggageSizes()
        {
            var availableSizes = Enum.GetValues<BaggageSize>()
                .Select(x => new AddBaggageVM() { Size = x.ToString() })
                .ToArray();
            
            return availableSizes;
        }
    }
}
