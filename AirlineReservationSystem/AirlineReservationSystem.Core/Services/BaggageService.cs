using AirlineReservationSystem.Core.Contracts;
using AirlineReservationSystem.Core.Models.User_Area;
using AirlineReservationSystem.Infrastructure.Models;
using AirlineReservationSystem.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace AirlineReservationSystem.Core.Services
{
    public class BaggageService : IBaggageService
    {

        private readonly IApplicatioDbRepository repo;

        public BaggageService(IApplicatioDbRepository _repo)
        {
            repo = _repo;
        }

        /// <summary>
        /// Gets the chosen from user baggage size, created a new model and adds it to the chosen booking.
        /// </summary>
        public async Task<bool> AddBaggageToBoooking(string BookingId, string PassengerId, AddBaggageVM model)
        {
            bool addedSuccessfully = false;

            try
            {
                var Size = Enum.GetValues<BaggageSize>().Where(x => x.Equals(model.Size)).FirstOrDefault();

                var baggage = new Baggage()
                {
                    Size = Size,
                    IsReportedLost = false,
                    BookingId = BookingId,
                    PassengerId = PassengerId
                };

                await repo.AddAsync<Baggage>(baggage);
                await repo.SaveChangesAsync();
                addedSuccessfully = true;
            }
            catch (Exception)
            {
                throw;
            }

            return addedSuccessfully;
        }

        /// <summary>
        /// Gets the enum of available baggage sizes
        /// </summary>
        public IEnumerable<AddBaggageVM> GetAvailableBaggageSizes()
        {
            var availableSizes = Enum.GetValues<BaggageSize>()
                .Select(x => new AddBaggageVM() { Size = x.ToString() })
                .ToArray();

            return availableSizes;
        }

        /// <summary>
        /// Gets all the baggage pieced for the given Booking and Passenger ID
        /// </summary>
        public async Task<IEnumerable<ReportLostBaggageVM>> GetBaggagesForBooking(string BookingId, string PassengerId)
        {
            return await repo.All<Baggage>()
                .Where(x => x.PassengerId == PassengerId)
                .Where(x => x.BookingId == BookingId)
                .Select(x => new ReportLostBaggageVM
                {
                    BaggageId = x.BaggageId,
                    IsReportedLost = x.IsReportedLost.ToString(),
                    Size = x.Size.ToString(),
                })
                .ToListAsync();
        }

        /// <summary>
        /// Sets IsReportedLost prop of the given BaggageId piece to true
        /// </summary>
        public async Task<bool> ReportAsLost(string BaggageId)
        {
            bool reportedSuccessfully = false;

            try
            {
                var baggage = await repo.GetByIdAsync<Baggage>(BaggageId);
                baggage.IsReportedLost = true;
                await repo.SaveChangesAsync();
                reportedSuccessfully = true;
            }
            catch (Exception)
            {
                throw;
            }

            return reportedSuccessfully;
        }
    }
}
