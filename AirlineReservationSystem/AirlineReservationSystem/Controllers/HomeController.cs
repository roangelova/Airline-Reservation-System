using AirlineReservationSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using System.Diagnostics;

namespace AirlineReservationSystem.Controllers
{
    public class HomeController : Controller
    {
        private readonly IDistributedCache cache;

        private readonly ILogger<HomeController> logger;

        public HomeController(ILogger<HomeController> _logger, IDistributedCache _cache)
        {
            logger = _logger;
            cache = _cache;

        }

        public async  Task<IActionResult> Index()
        {
            DateTime dateTime = DateTime.Now;

            var cachedDate = await cache.GetStringAsync("cachedTime");

            if (cachedDate == null)
            {
                cachedDate = dateTime.ToString();
                DistributedCacheEntryOptions options = new DistributedCacheEntryOptions()
                {
                    SlidingExpiration = TimeSpan.FromSeconds(20),
                    AbsoluteExpiration= DateTime.Now.AddSeconds(60)
                };

                await cache.SetStringAsync("cachedTime", cachedDate, options);
            }


            return View("Index", cachedDate);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}