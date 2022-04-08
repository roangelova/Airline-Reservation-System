using AirlineReservationSystem.Core.Contracts;
using AirlineReservationSystem.Core.Models.AdminArea.Aircraft;
using AirlineReservationSystem.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirlineReservationSystem.Core.Services
{
    public class AircraftService : IAircraftService
    {
        private readonly IApplicatioDbRepository repo;

        public AircraftService(IApplicatioDbRepository _repo)
        {
            repo = _repo;
        }
        public Task<AddAircraftVM> AddAircraft()
        {
            throw new NotImplementedException();
        }
    }
}
