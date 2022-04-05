using AirlineReservationSystem.Core.Models.Users;
using AirlineReservationSystem.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace AirlineReservationSystem.Core.Contracts
{
    public interface IUserService
    {
        Task<IEnumerable<UserListVM>> GetUsers();

        Task<UserEditVM> GetUserForEdit(string id);

        Task<bool> UpdateUser(UserEditVM model);

        Task<ApplicationUser> GetUserById(string id);
    }
}
