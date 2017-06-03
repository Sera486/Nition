using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ValueGeneration.Internal;
using OnlineCourses.Data;
using OnlineCourses.Models;
using OnlineCourses.Models.HomeViewModels;

namespace OnlineCourses.Controllers
{
    
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Course(int page=1, string search="",bool desc = false,int theme=0)
        {
            var pageSize = 5;
            //selecting courses with all info
            IQueryable<Course> source = _context.Courses
                .Include(course => course.Author)
                .Include(course => course.CourseThemes);//.ThenInclude(courseTheme => courseTheme.ThemeID);
            
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

        private List<Course> SearchCourse(List<Course> source, string searchStr, bool searchInDesc,
            int themeID)
        {
            if (!string.IsNullOrWhiteSpace(searchStr))
            {
                source = source.Where(c => c.Title.Contains(searchStr)).ToList();
                if (searchInDesc)
                    source = source.Where(c => c.Description.Contains(searchStr)).ToList();
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

        [HttpGet("Course/{id}")]
        public IActionResult CourseInfo(int id)
        {
            
            return View(_context.Courses.Find(id));
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
