using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Nition.Data;

namespace Nition.ViewComponents
{
    public class LessonListViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;

        public LessonListViewComponent(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync(int courseID, bool isPaid = false)
        {
            var course = await _context.Courses
                .Include(c => c.Lessons).ThenInclude(c => c.VideoBlocks).Include(c => c.Author).AsNoTracking()
                .FirstOrDefaultAsync(c => c.ID == courseID);
            ViewBag.isPaid = isPaid;
            return View(course);
        }
    }
}