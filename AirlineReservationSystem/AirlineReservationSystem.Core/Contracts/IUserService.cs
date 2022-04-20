using AirlineReservationSystem.Core.Models.Users;
using AirlineReservationSystem.Infrastructure.Models;

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
