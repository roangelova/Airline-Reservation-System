using AirlineReservationSystem.Core.Contracts;
using AirlineReservationSystem.Core.Models.User_Area;
using AirlineReservationSystem.Infrastructure.Models;
using AirlineReservationSystem.Infrastructure.Repositories;
using System.Globalization;

namespace AirlineReservationSystem.Core.Services
{
    public class PassengerService : IPassengerService
    {
        private readonly IApplicatioDbRepository repo;

        public PassengerService(IApplicatioDbRepository _repo)
        {
            repo = _repo;
        }
        public async Task<bool> RegisterPassenger(EditPassengerDataVM model)
        {
            var addedSuccessfully = false;

            var passenger = new Passenger()
            {
                DOB = DateTime.ParseExact(model.DateOfBirth, "g", CultureInfo.InvariantCulture),
                Nationality = model.Nationality,
                FirstName = model.FirstName,
                LastName = model.LastName,
                DocumentNumber = model.DocumentId
            };

            try
            {
                await repo.AddAsync(passenger);
                await repo.SaveChangesAsync();

             addedSuccessfully = true;
            }
            catch (Exception)
            {
            }

            return addedSuccessfully;

        }
    }
}
