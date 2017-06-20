using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Nition.Data;
using Nition.Models;

namespace Nition.Controllers
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

        public async Task<IActionResult> BestLecturers()
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

            dictionary = dictionary.OrderByDescending(d => d.Value).Take(6).ToDictionary(pair => pair.Key, pair => pair.Value);

            var lecturers = dictionary.Select(d => d.Key).ToList();

            return View(lecturers);
        }

        public async Task<IActionResult> About()
        {
            return View();
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
