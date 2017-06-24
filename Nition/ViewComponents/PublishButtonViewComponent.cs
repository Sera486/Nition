using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Nition.Data;
using Nition.Models;

namespace Nition.ViewComponents
{
    public class PublishButtonViewComponent:ViewComponent
    {
        private readonly ApplicationDbContext _context;
        
        public PublishButtonViewComponent(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync(int  courseID)
        {
            var course= await _context.Courses.Include(c => c.Author).AsNoTracking().FirstAsync(c=>c.ID==courseID);
            return View(course);
        }
    }
}
