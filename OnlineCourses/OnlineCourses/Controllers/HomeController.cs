﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineCourses.Data;
using OnlineCourses.Models;

namespace OnlineCourses.Controllers
{
    
    public class HomeController : Controller
    {
        private UserManager<ApplicationUser> _userManager;
        private ApplicationDbContext _context;
        public HomeController(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            if (user != null)
            {
                var role = await _userManager.GetRolesAsync(user);
                if (role[0] == RolesData.Admin)
                    return RedirectToAction(nameof(AdminController.Index), "Admin");
            }
            
            return View(await _context.Themes.ToListAsync());
        }

        public async Task<IActionResult> About()
        {
            var dictionary = new Dictionary<ApplicationUser, int>();
            foreach (var user in _context.ApplicationUser)
            {
                if (await _userManager.IsInRoleAsync(user, "Lecturer"))
                {
                    dictionary.Add(user, 0);
                }
            }
            foreach (var course in _context.Courses.Include(c => c.Subscriptions).Include(c => c.Author))
            {
                 dictionary[course.Author] = dictionary[course.Author] + course.Subscriptions.Count;
            }
            return View(dictionary);
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
