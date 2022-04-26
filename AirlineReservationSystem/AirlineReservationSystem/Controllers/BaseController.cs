using AirlineReservationSystem.Infrastructure.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AirlineReservationSystem.Controllers
{
    [Authorize]
    public class BaseController : Controller
    {
        protected readonly UserManager<ApplicationUser> userManager;

        public BaseController(UserManager<ApplicationUser> _userManager)
        {
            userManager = _userManager;
        }

        protected async Task<string> GetUserIdAsync()
        {
            var user = await userManager.GetUserAsync(this.User);
            return await userManager.GetUserIdAsync(user);
        }
    }
}
