using AirlineReservationSystem.Core.Contracts;
using AirlineReservationSystem.Core.Models.User_Area;
using AirlineReservationSystem.Infrastructure.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AirlineReservationSystem.Controllers
{
    public class BaggageController : BaseController
    {
        private readonly IBaggageService baggageService;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IPassengerService passengerService;

        public BaggageController(
            IBaggageService _baggageService,
            UserManager<ApplicationUser> _userManager,
            IPassengerService _passengerService)
        {
            baggageService = _baggageService;
            userManager = _userManager;
            passengerService = _passengerService;
        }
        public IActionResult AddBaggage()
        {
            var availableSizes = baggageService.GetAvailableBaggageSizes();

            ViewBag.AvailableSizes = availableSizes
                .Select(s => new SelectListItem()
                {
                    Text = s.Size,
                    Value = s.Size
                })
                .ToList();

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddBaggage(string id, AddBaggageVM model)
        {
            var user = await userManager.GetUserAsync(this.User);
            var currentUserId = await userManager.GetUserIdAsync(user);
            var PassengerId = await passengerService.GetPassengerId(currentUserId);

            bool success = await baggageService.AddBaggageToBoooking(id, PassengerId, model);

            if(success)
            {
                return RedirectToAction("MyBookings", "Passenger");
            }
            else
            {
                return View("CustomError");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetReportLostBaggage(string id)
        {
            var user = await userManager.GetUserAsync(this.User);
            var currentUserId = await userManager.GetUserIdAsync(user);
            var PassengerId = await passengerService.GetPassengerId(currentUserId);

            var UserBaggages = await baggageService.GetBaggagesForBooking(id, PassengerId);

            return View(UserBaggages);
        }

        [HttpPost]
        public async Task<IActionResult> ReportLostBaggage(string id)
        {
            var result = await baggageService.ReportAsLost(id);

            if (result)
            {
                return RedirectToAction("ReportLostBaggage");
            }
            else
            {
                return View("CustomError");
            }
        }

    }
}
