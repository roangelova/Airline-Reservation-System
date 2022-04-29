using AirlineReservationSystem.Infrastructure.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace AirlineReservationSystem.Controllers
{
    [Authorize]
    public class BaseController : Controller
    {
        protected readonly UserManager<ApplicationUser> userManager;
        private readonly IMemoryCache cache;

        public BaseController(
            UserManager<ApplicationUser> _userManager,
            IMemoryCache _cache)
        {
            userManager = _userManager;
            cache = _cache;
        }

        protected async Task<string> GetUserIdAsync()
        {
            ApplicationUser user;

            if (!this.cache.TryGetValue("user", out ApplicationUser data))
            {
                data = await userManager.GetUserAsync(this.User);

                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromSeconds(1000));

                this.cache.Set("user", data, cacheEntryOptions);
            }

            user = data;
            return user.Id;
        }
    }
}
