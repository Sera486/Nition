using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MedOk.Models;
using MedOk.Models.AccountViewModels;
using MedOk.Services;
using Newtonsoft.Json.Linq;

namespace MedOk.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger _logger;
        private readonly string _externalCookieScheme;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IOptions<IdentityCookieOptions> identityCookieOptions,
            ILoggerFactory loggerFactory)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _externalCookieScheme = identityCookieOptions.Value.ExternalCookieAuthenticationScheme;
            _logger = loggerFactory.CreateLogger<AccountController>();
        }
        
        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody]LoginViewModel model)
        {
            try
            {
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.Remember, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    _logger.LogInformation(1, "User logged in.");
                    return Json(new { result = true });
                }
                else
                {
                    throw new Exception(result.ToString());
                }
            }
            catch (Exception e)
            {
                _logger.LogWarning(6, "Invalid login attempt.");
                return Json(new { result = false, error = e.Message});
            }
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody]RegisterViewModel model)
        {
            try
            {
                ApplicationUser user;
                switch (model.Role)
                {
                    case "Patient" :
                        user = new Patient()
                        {
                            Records = new List<MedicalRecord>()
                        };
                        break;
                    case "Receptionist":
                        user = new Receptionist();
                        break;
                    case "Doctor":
                        user = new Doctor();
                        break;
                    default:
                        throw new Exception("Unknown user role");
                }
                user.UserName = model.Email;
                user.Email = model.Email;
                user.Name = model.FirstName;
                user.Surname = model.LastName;
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, model.Role);
                    _signInManager.SignInAsync(user, isPersistent: false).Wait();
                    _logger.LogInformation(3, "User created a new account with password.");
                    return Json(new { result = true });
                } else
                {
                    throw new Exception(result.ToString());
                }
            } catch (Exception e)
            {
                _logger.LogInformation(3, "User creation failed.");
                return Json(new { result = false, message = e.Message });
            }
        }

        //
        // POST: /Account/Logout
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            _logger.LogInformation(4, "User logged out.");
            return Json( new { result = true });
        }
    }
}
