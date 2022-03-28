using AirlineReservationSystem.Data;
using AirlineReservationSystem.Infrastructure.Common;

namespace AirlineReservationSystem.Infrastructure.Repositories
{
    public class ApplicatioDbRepository : Repository, IApplicatioDbRepository
    {
        public ApplicatioDbRepository(ApplicationDbContext context)
        {
            this.Context = context;
        }
    }
}
