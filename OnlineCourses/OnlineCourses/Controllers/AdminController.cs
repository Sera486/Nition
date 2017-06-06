using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OnlineCourses.Data;
using OnlineCourses.Models;
using OnlineCourses.Models.AdminViewModels;
using OnlineCourses.Models.CourseViewModels;
using OnlineCourses.Models.Enums;


namespace OnlineCourses.Controllers
{
    public class AdminController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        public AdminController(
            UserManager<ApplicationUser> userManager,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var courseCount = _context.Courses.Count(c => c.PublishStatus == PublishStatus.Proccesing);
            //TODO запилить это дело
            var comentsCount = 0;
            var model = new IndexViewModel{CoursesCount = courseCount,CommentsCount = comentsCount};
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Course(int page=1, string search="",bool global=false)
        {
            var pageSize = 5;
            //selecting courses with all info
            IQueryable<Course> source = _context.Courses
                .Include(course => course.Author)
                .Include(course => course.CourseThemes);
            
            //searching through courses
            var allItems =await SearchCourse( source, search,global).ToListAsync();
            
            //splitting into pages
            var count = allItems.Count;
            var pageItems = allItems.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            var pageViewModel = new PageViewModel(count, page, pageSize);

            var viewModel = new CourseViewModel
            {
                PageViewModel = pageViewModel,
                Courses = pageItems,
                SearchString = search
            };
            
            //filling themes selector
            var themes = _context.Themes.Select(t => new SelectListItem {Text = t.Name, Value = t.ID.ToString()}).ToList();
            ViewBag.Themes = themes;
            return View(viewModel);
        }

        private IQueryable<Course> SearchCourse(IQueryable<Course> source, string searchStr,bool isGlobal)
        {
            if (!isGlobal)
                source = source.Where(c => c.PublishStatus == PublishStatus.Proccesing);
            if (!string.IsNullOrWhiteSpace(searchStr))
            {
                searchStr = searchStr.ToLower();
                source =  source.Where(c => c.Title.ToLower().Contains(searchStr) || c.Description.ToLower().Contains(searchStr));
            }
            
            return source;
        }

        [HttpPost]
        public IActionResult PublishCourse(Course courseID, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            var course =_context.Courses.Find(courseID);
            course.PublishStatus=PublishStatus.Published;
            _context.Update(course);

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
