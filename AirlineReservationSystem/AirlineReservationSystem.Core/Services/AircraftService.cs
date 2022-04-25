using AirlineReservationSystem.Core.Contracts;
using AirlineReservationSystem.Core.Models.AdminArea.Aircraft;
using AirlineReservationSystem.Infrastructure.Models;
using AirlineReservationSystem.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace AirlineReservationSystem.Core.Services
{
    public class AircraftService : IAircraftService
    {
        private readonly IApplicatioDbRepository repo;

        public AircraftService(IApplicatioDbRepository _repo)
        {
            repo = _repo;
        }

        public async Task<bool> AddAircraft(AddAircraftVM model)
        {

            bool addedSuccessfully = false;

            var aircraft = new Aircraft()
            {
                Manufacturer = model.Manufacturer,
                Model = model.AircraftModel,
                Capacity = int.Parse(model.Capacity),
                ImageUrl = model.ImageUrl
            };


            try
            {
                await repo.AddAsync(aircraft);
                await repo.SaveChangesAsync();
                addedSuccessfully = true;
            }
            catch (Exception)
            {

            }

            return addedSuccessfully;

        }

        public async Task<IEnumerable<AddAircraftVM>> GetAllAircraft()
        {
            return await repo.All<Aircraft>()
                .Select(a =>
                new AddAircraftVM
                {
                    Manufacturer = a.Manufacturer,
                    AircraftModel = a.Model,
                    Capacity = a.Capacity.ToString(),
                    ImageUrl = a.ImageUrl,
                    Id = a.AircraftId

                })
                .ToListAsync();
        }
    }
}
