using AirlineReservationSystem.Core.Contracts;
using AirlineReservationSystem.Core.Models.User_Area;
using AirlineReservationSystem.Core.Models.Users;
using AirlineReservationSystem.Infrastructure.Models;
using AirlineReservationSystem.Infrastructure.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AirlineReservationSystem.Core.Services
{
    public class UserService : IUserService
    {
        private readonly IApplicatioDbRepository repo;

        public UserService(
            IApplicatioDbRepository _repo)
        {
            repo = _repo;
        }

        public async Task<ApplicationUser> GetUserById(string id)
        {
            return await repo.GetByIdAsync<ApplicationUser>(id);

        }

        public async Task<UserEditVM> GetUserForEdit(string id)
        {
            var user = await repo.GetByIdAsync<ApplicationUser>(id);

            return new UserEditVM()
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
            };

        }

        public async Task<IEnumerable<UserListVM>> GetUsers()
        {
            return await repo.All<ApplicationUser>()
                .Select(u =>
                new UserListVM
                {
                    Email = u.Email,
                    Id = u.Id,
                    Name = $"{u.FirstName} {u.LastName}"
                })
                .ToListAsync();
        }

        /// <summary>
        /// Once a passenger is created by the PassengerService, this method sets the Passenger Id in the User as well
        /// </summary>
        public async Task<bool> SetUserData(string userId, string PassengerId, EditPassengerDataVM model)
        {
            bool addedSuccessfully = false;
            try
            {
                var user = await repo.GetByIdAsync<ApplicationUser>(userId);

                user.PassengerId = PassengerId;
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;

                await repo.SaveChangesAsync();

                addedSuccessfully = true;
            }
            catch (Exception)
            {
                throw;
            }

            return addedSuccessfully;
        }

        public async Task<bool> UpdateUser(UserEditVM model)
        {
            bool result = false;

            try
            {
                var user = await repo.GetByIdAsync<ApplicationUser>(model.Id);

                if (user != null)
                {
                    user.FirstName = model.FirstName;
                    user.LastName = model.LastName;
                    await repo.SaveChangesAsync();


                    result = true;
                }

            }
            catch (Exception)
            {

                throw;
            }

            return result;
        }
    }
}
