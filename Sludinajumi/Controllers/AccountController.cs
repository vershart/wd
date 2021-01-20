using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Sludinajumi.Models;
using Sludinajumi.Api.Models;
using Sludinajumi.Api.Auth;
using Sludinajumi.Api.Data;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Hosting;

namespace Sludinajumi.Controllers
{
    [Authorize]
    public class AccountController: Controller
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;  
        private readonly SignInManager<ApplicationUser> _signInManager;
        //private readonly IEmailSender _emailSender;
        private readonly IJwtFactory _jwtFactory;
        private readonly JsonSerializerSettings _serializerSettings;
        private readonly JwtIssuerOptions _jwtOptions;
        private readonly SludinajumiContext _context;
        private readonly IHostingEnvironment _env;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            IJwtFactory jwtFactory, IOptions<JwtIssuerOptions> jwtOptions,
            SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager, SludinajumiContext context,
            IHostingEnvironment env)
        {
            _userManager = userManager;
            _jwtFactory = jwtFactory;
            _jwtOptions = jwtOptions.Value;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _context = context;
            _env = env;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> RegisterAdministrator()
        {
            if ((await _roleManager.RoleExistsAsync("Administrator")) == false)
            {
                IdentityRole userRole = new IdentityRole("User");
                IdentityRole adminRole = new IdentityRole("Administrator");
                if (!(await _roleManager.CreateAsync(userRole)).Succeeded)
                    throw new ApplicationException("Error while creating User role!");
                if (!(await _roleManager.CreateAsync(adminRole)).Succeeded)
                    throw new ApplicationException("Error while creating Administrator role!");
                
                if (_env.IsProduction()) {
                    ApplicationUser au = new ApplicationUser();
                    return View(au);
                }
                else
                    return RedirectToAction(nameof(RegisterTemporaryAccount));
            }
            else if ((await _userManager.GetUsersInRoleAsync("Administrator")).LongCount() != 0) {
                return RedirectToAction(nameof(RegisterTemporaryAccount));
            }
            else
                return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> RegisterAdministrator(AdministratorRegisterViewModel model)
        {
            if (model == null || ModelState.IsValid)
                return View(model);
            ApplicationUser au = new ApplicationUser {
                FirstName = model.FirstName,
                LastName = model.LastName,
                BirthDate = model.BirthDate,
                Email = model.Email,
                UserName = model.Email,
                RegistrationDate = DateTime.Now   
            };
            await _userManager.CreateAsync(au, model.Password);
            await _userManager.AddToRoleAsync(au, "Administrator");
            return RedirectToAction(nameof(Success));
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> RegisterTemporaryAccount()
        {

            if ((await _userManager.GetUsersInRoleAsync("Administrator")).LongCount() != 0)
                return RedirectToAction(nameof(HomeController.Index), "Home");

            ApplicationUser au = new ApplicationUser() {
                FirstName = "Artūrs",
                LastName = "Veršiņins",
                BirthDate = DateTime.Parse("1997/05/15"),
                Email = "po4teda@hotmail.com",
                UserName = "po4teda@hotmail.com",
                RegistrationDate = DateTime.Now
            };

            string password = GetRandomRassword();

            if ((await _userManager.CreateAsync(au, password)).Succeeded)
            {
                await _userManager.AddToRoleAsync(au, "Administrator");
                await _signInManager.PasswordSignInAsync(au, password, true, false);
                return View(new TemporaryAccountDataViewModel(au, password));
            }
            throw new ApplicationException("Error while creating temporary Administrator account!");
        }

        private string GetRandomRassword()
        {
            string availableChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz1234567890~!@#$%^&*()_+`-=/?.,><|\\[]{}:;";
            Random rnd = new Random();
            string password = new string(Enumerable
                .Repeat(availableChars, 25)
                .Select(str => str[rnd.Next(str.Length)])
                .ToArray());
            return password;
        }

        [AllowAnonymous]
        public IActionResult Success()
        {
            ViewData["Title"] = "Success!";
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
            ViewData["Title"] = "Lietotāju reģistrācija";
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            ViewData["Title"] = "Lietotāju autorizācija";
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (model == null || !ModelState.IsValid)
                return View(model);

            var user = new ApplicationUser {
                UserName = model.Email,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                BirthDate = model.BirthDate,
                Organization = model.Organization,
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                result = await _userManager.AddToRoleAsync(user, "User");
                if (result.Succeeded)
                    return RedirectToAction(nameof(Success));
            }

            foreach (IdentityError err in result.Errors)
                ModelState.AddModelError(err.Code, err.Description);

            return View(model);

        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            if (model == null || !ModelState.IsValid)
                return View(model);

            var identity = await GetClaimsIdentity(model.UserName, model.Password);
            if (identity == null) {
                ModelState.AddModelError(string.Empty, "Lietotāja konts ar tādu lietotāja vārdu vai paroli neeksistē!");
                return View(model);
            }
            
            var response = new
            {
                id = identity.Claims.Single(c => c.Type == "id").Value,
                auth_token = await _jwtFactory.GenerateEncodedToken(model.UserName, identity),
                expires_in = (int)_jwtOptions.ValidFor.TotalSeconds
            };

            var result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, true, lockoutOnFailure: false);

            if (result.Succeeded)
                return RedirectToAction(nameof(HomeController.Index), "Home");
            else
                return View(result);

        }

        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            ViewData["Title"] = "Atteikts";
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Manage()
        {
            ViewData["Title"] = "Lietotāja konts";
            var currentUser =  await _userManager.GetUserAsync(this.User);
            UserViewModel model = new UserViewModel {
                AccountInformation = currentUser,
                Role = string.Join(",", await _userManager.GetRolesAsync(currentUser)),
                Ads = _context.Ads.Where(ad => ad.CreatedById == currentUser.Id).ToList()
            };

            return View(model);
        } 

        private async Task<ClaimsIdentity> GetClaimsIdentity(string userName, string password)
        {
            if (!string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(password))
            {
                var userToVerify = await _userManager.FindByNameAsync(userName);

                if (userToVerify != null)
                {
                    if (await _userManager.CheckPasswordAsync(userToVerify, password))
                        return await Task.FromResult(_jwtFactory.GenerateClaimsIdentity(userName,userToVerify.Id));
                }
            }
            return await Task.FromResult<ClaimsIdentity>(null);
        }

    }
}