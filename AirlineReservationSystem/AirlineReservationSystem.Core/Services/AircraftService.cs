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

        /// <summary>
        /// Adds a new aircraft to the database and returns bool whether the operation was successful
        /// </summary>
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
                throw;
            }

            return addedSuccessfully;

        }


        /// <summary>
        /// Checks if there are any flight at the moment using the aircraft with the gibven Id. Used for error handling purposes.
        /// </summary>
        public async Task<bool> CheckIfInUse(string AircraftId)
        {
            var used = false;

            var flights = await repo.All<Flight>()
                .Where(f => f.FlightStatus == Infrastructure.Status.Scheduled)
                .ToListAsync();

            if (flights.Any(x => x.AircraftID == AircraftId))
            {
                used = true;
                return used;
            }

            return used;
        }

        /// <summary>
        /// Gets all available aircraft in the data base. Currently only used for visualization purposes in admin
        /// area
        /// </summary>
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

        public async Task<IEnumerable<GetAircraftDataVM>> GetAllAircraftForCancellation()
        {
            var flights = await repo.All<Flight>().ToListAsync();

            var aircraft = await repo.All<Aircraft>()
                .Select(a =>
                new GetAircraftDataVM
                {
                    AircraftId = a.AircraftId,
                    AircraftMakeAndModel = $"{a.Manufacturer} {a.Model}"
                })
                .ToListAsync();

            foreach (var a in aircraft)
            {
                if (flights.Any(x => x.AircraftID == a.AircraftId))
                {
                    a.InUse = "Yes";
                }
                else
                {
                    a.InUse = "No";
                }
            }

            return aircraft;
        }

        public async Task<bool> RemoveAircraft(string AircraftId)
        {
            bool removed = false;

            try
            {
                await repo.DeleteAsync<Aircraft>(AircraftId);
                await repo.SaveChangesAsync();
                removed = true;
            }
            catch (Exception)
            {

                throw;
            }

            return removed;
        }
    }
}
