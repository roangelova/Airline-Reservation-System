using AirlineReservationSystem.Core.Contracts;
using AirlineReservationSystem.Core.Models.Users;
using AirlineReservationSystem.Infrastructure.Models;
using AirlineReservationSystem.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirlineReservationSystem.Core.Services
{
    public  class UserService : IUserService
    {
        private readonly IApplicatioDbRepository repo;

        public UserService(IApplicatioDbRepository _repo)
        {
            repo = _repo;
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
    }
}
