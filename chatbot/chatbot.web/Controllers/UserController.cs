using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using IndianBank_ChatBOT.Models;
using IndianBank_ChatBOT.Utils;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace IndianBank_ChatBOT.Controllers
{
    public class UserController : Controller
    {
        private readonly AppDbContext _dbContext;
        private readonly AppSettings _appSettings;

        public UserController(IOptions<AppSettings> appsettings, AppDbContext _dbContext)
        {
            this._dbContext = _dbContext;
            _appSettings = appsettings.Value;
        }

        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        [AllowAnonymous]
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult> Login([Bind] UserViewModel vm)
        {
            if (ModelState.IsValid)
            {
                var user = _dbContext.Users.FirstOrDefault(u => u.Username.ToLower() == vm.Username.ToLower());

                if (user == null)
                {
                    ViewBag.ErrorMessage = "User does not exist!";
                    return View(vm);
                }
                else
                {
                    var hashedPassword = HashingUtils.Hash(vm.Password, user.Id);

                    var isValidUser = user.Password == hashedPassword;

                    if (isValidUser)
                    {
                        var userClaims = new List<Claim>()
                        {
                            new Claim(ClaimTypes.Name, user.Name)
                        };

                        var identityClaim = new ClaimsIdentity(userClaims, "User Identity");

                        var userPrincipal = new ClaimsPrincipal(new[] { identityClaim });

                        var loginExpiryMinutes = Convert.ToInt32(_appSettings.LoginExpiryMinutes);

                        await HttpContext.SignInAsync(userPrincipal, new AuthenticationProperties
                        {
                            IsPersistent = true,
                            ExpiresUtc = DateTime.UtcNow.AddMinutes(loginExpiryMinutes)
                        });

                        return RedirectToAction("RealTimeDashboard", "Dashboard");
                    }
                    else
                    {
                        ViewBag.ErrorMessage = "Username or Password is incorrect!";
                        return View(vm);
                    }
                }
            }

            ViewBag.ErrorMessage = "Invalid inputs!";
            return View(vm);
        }


        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult> Logout()
        {
            await HttpContext.SignOutAsync("CookieAuthentication");
            return RedirectToAction("Login");
        }
    }
}
