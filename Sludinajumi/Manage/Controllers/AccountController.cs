using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Sludinajumi.Api.Models;
using Sludinajumi.Manage.Models;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Sludinajumi.Manage.Controllers
{
    [Area("Manage")]
    [Authorize(Roles = "Administrator")]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public AccountController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            ViewBag.usersCount = userManager.Users.LongCount();
            ViewBag.administratorsCount = (await userManager.GetUsersInRoleAsync("Administrator")).LongCount();
            ViewData["Title"] = "Lietotāju pārvalde";
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Show(string id = "")
        {
            if (id == "")
                return RedirectToAction(nameof(ShowAll));

            var u = await userManager.FindByIdAsync(id);
            var roles = await userManager.GetRolesAsync(u);
            ViewBag.userData = u;
            ViewBag.userRoles = string.Join(", ", roles.ToArray());
            ViewData["Title"] = $"Konta rediģēšana";
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string userId)
        {
            var result =  await userManager.DeleteAsync(userManager.Users.FirstOrDefault(u => u.Id == userId));
            if (result.Succeeded)
                return RedirectToAction(nameof(ShowAll));
            else
                return RedirectToAction(nameof(Show), userId);
        }

        [HttpPost]
        public async Task<IActionResult> ChangeEmail(string userId, string newEmail)
        {
            var user = await userManager.FindByIdAsync(userId);
            user.UserName = newEmail;
            user.Email = newEmail;
            var result = await userManager.UpdateAsync(user);
            if (result.Succeeded)
                return RedirectToAction(nameof(Show), new { id = userId });
            else
                return RedirectToAction(nameof(ShowAll));
        }

        [HttpPost]
        public async Task<IActionResult> ChangeUserGroup(string userId, string userRole)
        {
            ApplicationUser user = await userManager.FindByIdAsync(userId);
            IList<string> currentRoles = await userManager.GetRolesAsync(user);
            var result = await userManager.RemoveFromRolesAsync(
                user,
                currentRoles
            );
            result = await userManager.AddToRoleAsync(user, userRole);
            if (result.Succeeded)
                return RedirectToAction(nameof(Show), new { id = userId });
            else
                return RedirectToAction(nameof(ShowAll));
        }

        [HttpGet]
        public async Task<IActionResult> ShowAll(int offset = 0)
        {
            var users = userManager.Users.Select(u => new ApplicationUserListViewModel {
                Id = u.Id,
                FirstName = u.FirstName,
                LastName = u.LastName,
                RegistrationDate = u.RegistrationDate,
                EmailAdress = u.Email,
            }).ToList();

            foreach (var user in users)
            {
                var u = await userManager.FindByIdAsync(user.Id);
                var roles = await userManager.GetRolesAsync(u);
                user.RoleName = string.Join(", ", roles.ToArray());
            }

            if (offset < users.Count)
                offset = 0;

            int pageSize = 5;
            while (offset + pageSize > users.Count)
                pageSize--;

            ViewBag.usersData = users.GetRange(offset, pageSize);    
            ViewData["Title"] = "Lietotāju prvalde";    
            return View();
        }

    }
}