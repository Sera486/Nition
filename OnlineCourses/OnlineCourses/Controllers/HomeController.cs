using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        public async Task<IActionResult> Course(int page=1, string search="",bool searchInDesc = false,int theme=-1)
        {
            int pageSize = 5; 

            IQueryable<Course> source = _context.Courses.Include(x => x.Author);
            
            var allItems = source.Where(c => c.Title.Contains(search)).ToImmutableHashSet();//.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
            if (searchInDesc == true)
            {
                allItems.Union(source.Where(c => c.Description.Contains(search)));
            }

            var count = allItems.Count;
            var pageItems = allItems.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            PageViewModel pageViewModel = new PageViewModel(count, page, pageSize);
            CourseViewModel viewModel = new CourseViewModel
            {
                PageViewModel = pageViewModel,
                Courses = pageItems,
                SearchString = search,
                SearchInDescription = searchInDesc,
                ThemeID = theme
            };

            return View(viewModel);
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
