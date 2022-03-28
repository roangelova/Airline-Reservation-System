using AirlineReservationSystem.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AirlineReservationSystem.Areas.Admin.Controllers
{
    [Authorize(Roles = UserConstants.Role.AdministratorRole)]
    [Area("Admin")]
    public class BaseController : Controller
    {
        


    }
}
