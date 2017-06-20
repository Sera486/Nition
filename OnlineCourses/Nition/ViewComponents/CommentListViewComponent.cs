using System;
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
    public class CommentListViewComponent:ViewComponent
    {
        private readonly ApplicationDbContext _context;
        
        public CommentListViewComponent(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync(int courseID)
        {
            var course = await _context.Courses
                .Include(c => c.Comments).ThenInclude(c => c.User).FirstOrDefaultAsync(c=>c.ID==courseID);
            return View(course);
        }
    }
}
