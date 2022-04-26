using AirlineReservationSystem.Core;
using AirlineReservationSystem.Core.Constants;
using AirlineReservationSystem.Core.Contracts;
using AirlineReservationSystem.Core.Models.AdminArea.Users;
using AirlineReservationSystem.Core.Models.Users;
using AirlineReservationSystem.Infrastructure.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;
using System.Threading.Tasks;

namespace AirlineReservationSystem.Areas.Admin.Controllers
{
   [Authorize(Roles = UserConstants.Role.AdministratorRole)]
    public class UserController : BaseController
    {
        private readonly RoleManager<IdentityRole> roleManager;

        private readonly UserManager<ApplicationUser> userManager;

        private readonly IUserService userService;

        public UserController(
            RoleManager<IdentityRole> _roleManager,
            UserManager<ApplicationUser> _userManager,
            IUserService _userService)
        {
            roleManager = _roleManager;
            userManager = _userManager;
            userService = _userService;
        }
        public IActionResult Home()
        {
            return View();
        }


        public async Task<IActionResult> ManageUsers()
        {
            var users = await userService.GetUsers();

            return View(users);

        }

        public async Task<IActionResult> Roles(string id)
        {
            var user = await userService.GetUserById(id);

            var model = new UserRolesVM()
            {
                UserId = user.Id,
                Name = $"{user.FirstName} {user.LastName}"
            };

            ViewBag.RoleItems = roleManager.Roles
                  .ToList()
                  .Select(r => new SelectListItem()
                  {
                      Text = r.Name,
                      Value = r.Name,
                      Selected = userManager.IsInRoleAsync(user, r.Name).Result

                  }).ToList();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Roles(UserRolesVM model)
        {
            var user = await userService.GetUserById(model.UserId);
            var userRoles = await userManager.GetRolesAsync(user);
            await userManager.RemoveFromRolesAsync(user, userRoles);

            if (model.RoleNames?.Length > 0)
            {
                await userManager.AddToRolesAsync(user, model.RoleNames);
            }

            return RedirectToAction(nameof(ManageUsers));
        }

        public async Task<IActionResult> Edit(string id)
        {
            var model = await userService.GetUserForEdit(id);

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(UserEditVM model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (await userService.UpdateUser(model))
            {
                ViewData[MessageConstant.SuccessMessage] = "Update was successful";
            }
            else
            {
                ViewData[MessageConstant.SuccessMessage] = "Something went wrong";
            }

            return View(model);
        }

        public async Task<IActionResult> CreateRole()
        {
            //await roleManager.CreateAsync(new IdentityRole()
            //{
            //    Name = UserConstants.Role.AdministratorRole
            //});

            return Ok();
        }
    }
}
