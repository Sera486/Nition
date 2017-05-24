﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OnlineCourses.Data;
using OnlineCourses.Models;
using OnlineCourses.Models.AccountViewModels;
using OnlineCourses.Services;

namespace OnlineCourses.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IEmailSender _emailSender;
        private readonly ILogger _logger;
        private readonly string _externalCookieScheme;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IOptions<IdentityCookieOptions> identityCookieOptions,
            IEmailSender emailSender,
            ILoggerFactory loggerFactory,
            IServiceProvider serviceProvider)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _externalCookieScheme = identityCookieOptions.Value.ExternalCookieAuthenticationScheme;
            _emailSender = emailSender;
            _logger = loggerFactory.CreateLogger<AccountController>();
            _roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        }

        //
        // POST: /Account/Login
        [HttpPost("/Account/Login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody]LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Login, model.Password, model.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    return Json(new { result = true});
                }
                else
                {
                _logger.LogWarning(6, "Invalid login attempt.");
                return Json(new { result = false, error=result.ToString()});
                }
            }

            // If we got this far, something failed, redisplay form
            return Json(new { result = false});
        }

        //
        // POST: /Account/Register
        [HttpPost("/Account/Register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody]RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.Login, Email = model.Email, FirstName = model.FirstName, LastName = model.LastName };
                //user.Roles = Role;
                var result = await _userManager.CreateAsync(user, model.Password);
                switch (model.Role)
                {
                    case "Lecturer":
                        await _userManager.AddToRoleAsync(user, _roleManager.FindByNameAsync("Lecturer").Result.Name);
                        break;
                    case "Student":
                        await _userManager.AddToRoleAsync(user, _roleManager.FindByNameAsync("Student").Result.Name);
                        break;
                }
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    _logger.LogInformation(3, "User created a new account with password.");
                    return Json(new { result = true});
                }
            }

            // If we got this far, something failed, redisplay form
            return Json(new { result = false});
        }

        //
        // POST: /Account/Logout
        [HttpPost("/Account/Logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            _logger.LogInformation(4, "User logged out.");
            return Json(new { result = "true"});
        }
    }
}
