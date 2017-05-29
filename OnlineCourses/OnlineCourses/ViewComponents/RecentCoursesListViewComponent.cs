using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineCourses.Data;
using OnlineCourses.Models;

namespace OnlineCourses.ViewComponents
{
    public class RecentCoursesListViewComponent:ViewComponent
    {
        private readonly ApplicationDbContext _context;

        public RecentCoursesListViewComponent(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync(
            int maxPriority, bool isDone)
        {
            var items = await GetItemsAsync(maxPriority, isDone);
            return View(items);
        }
        private Task<List<Course>> GetItemsAsync(int maxPriority, bool isDone)
        {
            return _context.Courses.OrderByDescending(c=>c.Subscriptions.Count).Take(12).ToListAsync(); //.Select(e=>new CoursePreviewViewModel(){Author = e.Author.FullName,ImageURL = e.ImageURL,Title = e.Name})
        }
    }
}
