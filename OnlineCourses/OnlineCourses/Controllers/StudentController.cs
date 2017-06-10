using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineCourses.Data;
using OnlineCourses.Models;


namespace OnlineCourses.Controllers
{
    [Produces("application/json")]
    [Route("api/Student")]
    [Authorize(Roles = RolesData.Student)]
    public class StudentController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public StudentController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        #region Subcription

        private  Subscription GetSubscription(int courseId)
        {
            var user =  GetCurrentUserAsync().Result;
            var index = _context.Subscriptions.FirstOrDefault(e => e.User.Id == user.Id && e.Course.ID == courseId);
            return index;
        }

        [HttpPost]
        public IActionResult Subscribe(Course course, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            var user = _context.ApplicationUser.Include(u => u.Subscriptions).First(u => u.Id == GetCurrentUserAsync().Result.Id);
            if (!user.Subscriptions.Exists(s => s.Course == course))
            {
                var subscription = new Subscription {Course = course, User = user, SubscriptionDate = DateTime.Now};
                _context.Subscriptions.Update(subscription);
            }
            return RedirectToLocal(returnUrl);
        }

        [HttpPost]
        public IActionResult UnSubscribe(Course course, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            var user = _context.ApplicationUser.Include(u => u.Subscriptions).First(u => u.Id == GetCurrentUserAsync().Result.Id);
            if (user.Subscriptions.Exists(s => s.Course == course))
            {
                var c = user.Subscriptions.First(s => s.Course == course&&s.User==user);
                _context.Subscriptions.Remove(c);
            }
            return RedirectToLocal(returnUrl);
        }

        #endregion

        #region Helpers

        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
        }

        private Task<ApplicationUser> GetCurrentUserAsync()
        {
            return _userManager.GetUserAsync(HttpContext.User);
        }

        #endregion

    }
}