using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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

        [HttpPost("/Subscribe")]
        public async Task<IActionResult> Subscribe([FromBody] int id)
        {
            if (GetSubscription(id)==null)
            {
                _context.Subscriptions.Add(new Subscription
                {
                    Course = _context.Find<Course>(id),
                    User = GetCurrentUserAsync().Result
                });
                await _context.SaveChangesAsync();
                return Json(new { result = true });
            }
            return Json(new { result = false, message="User is already subscribing this course" });
        }

        [HttpPost("/UnSubscribe")]
        public async Task<IActionResult> UnSubscribe([FromBody] int id)
        {
            var index = GetSubscription(id);
            if (index!=null)
            {
                _context.Subscriptions.Remove(index);
                await _context.SaveChangesAsync();
                return Json(new { result = true });
            }
            return Json(new { result = false, message = "User doesn't subscribing this course" });
        }

        [HttpPost("/SubscriptionStatus")]
        public  IActionResult GetSubscriptionStatus([FromBody] int id)
        {
            return Json(GetSubscription(id) != null ? new { result = "subscribing" } : new { result = "unsubscribing" });
        }

        [HttpGet("/SubscriptionList")]
        public IActionResult GetSubscriptionList()
        {
            var user = GetCurrentUserAsync().Result;
            var subscribedCourses=_context.Subscriptions.Where(e => e.User == user);
            return Json(subscribedCourses);

        }
        #endregion

        private Task<ApplicationUser> GetCurrentUserAsync()
        {
            return _userManager.GetUserAsync(HttpContext.User);
        }

        private bool ApplicationUserExists(string id)
        {
            return _context.ApplicationUser.Any(e => e.Id == id);
        }
    }
}