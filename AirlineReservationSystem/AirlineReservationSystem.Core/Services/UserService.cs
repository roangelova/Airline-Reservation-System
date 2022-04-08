using AirlineReservationSystem.Core.Contracts;
using AirlineReservationSystem.Core.Models.Users;
using AirlineReservationSystem.Infrastructure.Models;
using AirlineReservationSystem.Infrastructure.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Web.Mvc;

namespace AirlineReservationSystem.Core.Services
{
    public  class UserService : IUserService
    {
        private readonly IApplicatioDbRepository repo;


        public UserService(IApplicatioDbRepository _repo)
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

        public async Task<bool> UpdateUser(UserEditVM model)
        {
            bool result = false;
            var user = await repo.GetByIdAsync<ApplicationUser>(model.Id);

            if (user != null)
            {
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                await repo.SaveChangesAsync();


                result = true;
            }


            return result;
        }
    }
}
