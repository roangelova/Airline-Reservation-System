using AirlineReservationSystem.Core.Contracts;
using AirlineReservationSystem.Core.Models.User_Area;
using AirlineReservationSystem.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirlineReservationSystem.Core.Services
{
    public class BookingService : IBookingService
    {
        private readonly IApplicatioDbRepository repo;

        public BookingService(IApplicatioDbRepository _repo)
        {
            repo = _repo;
        }

        public Task<IEnumerable<AvailableFlightsVM>> GetAllAvailableFlight()
        {
            throw new NotImplementedException();
        }
    }
}
