using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Nition.Data;
using Nition.Models;
using Nition.Models.ManageViewModels;

namespace Nition.ViewComponents
{
    public class UserCoursesListViewComponent:ViewComponent
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public UserCoursesListViewComponent(ApplicationDbContext context,UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync(string userID, int page=1)
        {
            var pageSize = 5;
            List<Course> items=new List<Course>();
            if (User.IsInRole(RolesData.Student))
            {
                var user =  _context.ApplicationUser.Include(a => a.Subscriptions).ThenInclude(s => s.Course)
                    .ThenInclude(s => s.Author)
                    .Include(u => u.SharingUsers).ThenInclude(fm => fm.User).ThenInclude(u => u.Subscriptions)
                    .ThenInclude(s => s.Course).ThenInclude(c => c.Author)
                    .First(c => c.Id == userID);
                items=user.ValidSubscriptions().Select(c => c.Course).ToList();
            }
            if (User.IsInRole(RolesData.Lecturer))
            {
                var user = _context.ApplicationUser.Include(s => s.CreatedCourses)
                    .ThenInclude(s => s.Author)
                    .First(c => c.Id == userID);
                items = user.CreatedCourses;
            }
            
            //splitting into pages
            var count = items.Count;
            var pageItems = items.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            var pageViewModel = new PageViewModel(count, page, pageSize);

            var viewModel = new UserAccountCourseListViewModel
            {
                PageViewModel = pageViewModel,
                Courses = pageItems,
                UserID = userID
            };
            return View(viewModel);
        }
    }
}
