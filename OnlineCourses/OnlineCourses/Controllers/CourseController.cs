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
        public async Task<IActionResult> Index(int page=1, string search="",bool desc = false,int theme=0)
        {
            var pageSize = 5;
            //selecting courses with all info
            IQueryable<Course> source = _context.Courses
                .Include(course => course.Author)
                .Include(course => course.CourseThemes);
            
            //searching through courses
            var allItems = SearchCourse( source.ToList(), search, desc, theme);//.ToListAsync();
            
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

        private List<Course> SearchCourse(List<Course> source, string searchStr, bool searchInDesc, int themeID)
        {
            if (!string.IsNullOrWhiteSpace(searchStr))
            {
                source = searchInDesc
                    ? source.Where(c => c.Title.Contains(searchStr) || c.Description.Contains(searchStr)).ToList()
                    : source.Where(c => c.Title.Contains(searchStr)).ToList();
                if (themeID != 0)
                    source = source.Where(c => c.CourseThemes.Exists(e => e.ThemeID == themeID)).ToList();
            }
            else
            {
                if (themeID != 0)
                    source = source.Where(c => c.CourseThemes.Exists(e => e.ThemeID == themeID)).ToList();
            }
            return source;
        }

        [HttpGet]
        public IActionResult CourseInfo(int id)
        {
            return View(_context.Courses.Find(id));
        }

        [HttpGet("Course/{id}")]
        public async Task<IActionResult> BuyingCourse(int ID)
        {
            var user = await GetCurrentUserAsync();
            if (user != null /*&& User.IsInRole("Student")*/)
            {
                var source = _context.Courses.Include(c=>c.Author);
                Course course = source.Where(c => c.ID == ID).ToList()[0];
                if (_context.Subscriptions.Where(e => e.User == user && e.Course == course).ToList().Count == 0)
                {
                    return View(course);
                }
                else
                {
                    return View(course);
                }
            }
            return View();
        }

        private Task<ApplicationUser> GetCurrentUserAsync()
        {
            return _userManager.GetUserAsync(HttpContext.User);
        }
    }
}
