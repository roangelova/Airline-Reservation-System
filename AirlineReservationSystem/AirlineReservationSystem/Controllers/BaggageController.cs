﻿using AirlineReservationSystem.Core.Contracts;
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
        private readonly IPassengerService passengerService;
        UserManager<ApplicationUser> userManager;

        public BaggageController(
            IBaggageService _baggageService,
            IPassengerService _passengerService,
            UserManager<ApplicationUser> _userManager) : base(_userManager)
        {
            baggageService = _baggageService;
            passengerService = _passengerService;
            userManager = _userManager;
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
            var currentUserId = await GetUserIdAsync();
            var PassengerId = await passengerService.GetPassengerId(currentUserId);

            if (PassengerId == "")
            {
                return View("CustomError");
            };

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
            var currentUserId = await GetUserIdAsync();
            var PassengerId = await passengerService.GetPassengerId(currentUserId);

            if (PassengerId == "")
            {
                return View("CustomError");
            };

            var UserBaggages = await baggageService.GetBaggagesForBooking(id, PassengerId);

            return View("ReportLostBaggage", UserBaggages);
        }

        [HttpPost]
        public async Task<IActionResult> ReportBaggageAsLlost(string id)
        {
            var result = await baggageService.ReportAsLost(id);

            if (result)
            {
                return RedirectToAction("GetReportLostBaggage");
            }
            else
            {
                return View("CustomError");
            }
        }

    }
}
