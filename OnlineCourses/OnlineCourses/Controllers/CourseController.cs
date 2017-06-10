using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OnlineCourses.Data;
using OnlineCourses.Models;
using OnlineCourses.Models.CourseViewModels;
using OnlineCourses.Models.Enums;


namespace OnlineCourses.Controllers
{
    public class CourseController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        public CourseController(
            UserManager<ApplicationUser> userManager,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int page=1, string search="",bool desc = true,int theme=0)
        {
            var pageSize = 5;
            //selecting courses with all info
            IQueryable<Course> source = _context.Courses
                .Include(course => course.Author)
                .Include(course => course.CourseThemes);
            
            //searching through courses
            var allItems =await SearchCourse( source, search, desc, theme).ToListAsync();
            
            //splitting into pages
            var count = allItems.Count;
            var pageItems = allItems.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            var pageViewModel = new PageViewModel(count, page, pageSize);

            var viewModel = new CourseViewModel
            {
                PageViewModel = pageViewModel,
                Courses = pageItems,
                SearchString = search,
                SearchInDescription = desc,
                ThemeID = theme
            };
            
            //filling themes selector
            var themes = _context.Themes.Select(t => new SelectListItem {Text = t.Name, Value = t.ID.ToString()}).ToList();
            ViewBag.Themes = themes;
            return View(viewModel);
        }

        private IQueryable<Course> SearchCourse(IQueryable<Course> source, string searchStr, bool searchInDesc, int themeID)
        {
            source = source.Where(c => c.PublishStatus == PublishStatus.Published);
            if (!string.IsNullOrWhiteSpace(searchStr))
            {
                searchStr = searchStr.ToLower();
                source = searchInDesc
                    ? source.Where(c => c.Title.ToLower().Contains(searchStr) || c.Description.ToLower().Contains(searchStr))
                    : source.Where(c => c.Title.ToLower().Contains(searchStr));
                if (themeID != 0)
                    source = source.Where(c => c.CourseThemes.Exists(e => e.ThemeID == themeID));
            }
            else
            {
                if (themeID != 0)
                    source = source.Where(c => c.CourseThemes.Exists(e => e.ThemeID == themeID));
            }
            return source;
        }

        [HttpGet]
        public IActionResult CourseInfo(int id)
        {
            return View(_context.Courses.Find(id));
        }
        
        public IActionResult Payment(int courseID)
        {
            return View(_context.Courses.Include(c => c.Author).Where(c => c.ID == courseID).ToList()[0]);
        }

        [HttpPost]
        public async Task<IActionResult> Buy(int courseID)
        {
                var sub = new Subscription()
                {
                    Course = _context.Courses.Find(courseID),
                    User = GetCurrentUserAsync().Result,
                    SubscriptionDate = DateTime.Today
                };
                _context.Subscriptions.Add(sub);
                await _context.SaveChangesAsync();
            return RedirectToAction("Payment");
        }

        [HttpGet("Course/{id}")]
        public async Task<IActionResult> BuyingCourse(int ID)
        {
            
            var user = await GetCurrentUserAsync();
            var source = _context.Courses.Include(c => c.Author)
                .Include(c => c.Lessons)
                .Include(c => c.Subscriptions)
                .Include(c=>c.Comments).ThenInclude(c=>c.User);
            Course course = source.Where(c => c.ID == ID).ToList()[0];
            ApplicationUser courseAuthor = _context.ApplicationUser.Where(author => author == course.Author)
                .ToList()[0];

            if (user != null)
            {
                user = _context.ApplicationUser.Include(c => c.Subscriptions).ThenInclude(s=>s.Course).First(c => c.Id == user.Id);
                if (user.Subscriptions.Count(e => e.Course == course) == 0)
                {
                    var viewModel = new BuyingCourseViewModel()
                    {
                        Course = course,
                        Paid = false,
                        IsAuthor = user == courseAuthor,
                        IsStudent = User.IsInRole("Student")
                    };
                    return View(viewModel);
                }
                else
                {
                    var viewModel = new BuyingCourseViewModel()
                    {
                        Course = course,
                        Paid = true,
                        IsAuthor = user == courseAuthor
                    };
                    return View(viewModel);
                }
            }
            else
            {
                var viewModel = new BuyingCourseViewModel()
                {
                    Course = course,
                    Paid = false,
                    IsAuthor = false,
                    IsStudent = false
                };
                return View(viewModel);
            }
            return View();
        }

        private Task<ApplicationUser> GetCurrentUserAsync()
        {
            return _userManager.GetUserAsync(HttpContext.User);
        }

        [HttpPost]
        public async Task<IActionResult> AddComment(int courseID, string commentText,string returnUrl=null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            var course = _context.Courses.Include(c => c.Comments).First(c=>c.ID==courseID);
            if (!string.IsNullOrWhiteSpace(commentText))
            {
                var user = await GetCurrentUserAsync();
                course.Comments.Add(new Comment {Date = DateTime.Now, Text = commentText, User = user});
                _context.Update(course);
                await _context.SaveChangesAsync();
            }
            return RedirectToLocal(returnUrl);
        }

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
    }
}
