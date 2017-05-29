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

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var items = await GetItemsAsync();
            return View(items);
        }
        private Task<List<Course>> GetItemsAsync()
        {
            return _context.Courses.OrderByDescending(c=>c.Subscriptions.Count).Take(12).ToListAsync();
        }
    }
}
