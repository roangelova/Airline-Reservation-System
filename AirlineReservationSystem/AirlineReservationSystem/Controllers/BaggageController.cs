using AirlineReservationSystem.Core.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AirlineReservationSystem.Controllers
{
    public class BaggageController : BaseController
    {
        private readonly IBaggageService baggageService;

        public BaggageController(IBaggageService _baggageService)
        {
            baggageService = _baggageService;
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
    }
}
