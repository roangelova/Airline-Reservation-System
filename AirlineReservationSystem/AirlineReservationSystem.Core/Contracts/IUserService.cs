using AirlineReservationSystem.Core.Models.Users;
using AirlineReservationSystem.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirlineReservationSystem.Core.Contracts
{
    public interface IUserService
    {
        Task<IEnumerable<UserListVM>> GetUsers();
    }
}
