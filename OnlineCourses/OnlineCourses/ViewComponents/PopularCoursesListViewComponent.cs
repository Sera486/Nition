using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineCourses.Data;
using OnlineCourses.Models;
using OnlineCourses.Models.Enums;

namespace OnlineCourses.ViewComponents
{
    public class PopularCoursesListViewComponent:ViewComponent
    {
        private readonly ApplicationDbContext _context;

        public PopularCoursesListViewComponent(ApplicationDbContext context)
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
            return _context.Courses.Where(c=>c.PublishStatus==PublishStatus.Published).OrderByDescending(c=>c.Subscriptions.Count).Take(12).ToListAsync();
        }
    }
}
