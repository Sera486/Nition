using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineCourses.Data;
using OnlineCourses.Models;
using OnlineCourses.Models.StudentViewModels;


namespace OnlineCourses.Controllers
{
    [Authorize(Roles = RolesData.Student)]
    public class StudentController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public StudentController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        #region Family sharring

        [HttpGet]
        public async Task<IActionResult> FamilyList(int page =1,string search=null)
        {
            var user = await _context.ApplicationUser
                .Include(u => u.FamilyMembers).ThenInclude(fm=>fm.Member)
                .FirstAsync(u => u.Id == GetCurrentUserAsync().Result.Id);

            var pageSize = 15;

            //selecting all students
            var studentRoleId= _context.Roles.First(r => r.Name == RolesData.Student).Id;
            var source = _context.ApplicationUser.Include(u => u.Roles)
                .Where(u => u.Roles.Any(r => r.RoleId == studentRoleId));
            //except current user
            source =source.Where(u=>u!=user);
            
            //searching through users
            var allItems = await SearchUser(source, search).ToListAsync();

            //splitting into pages
            var count = allItems?.Count;
            var pageItems = allItems.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            var pageViewModel = new PageViewModel(count.GetValueOrDefault(), page, pageSize);

            var viewModel = new FamilyListViewModel
            {
                PageViewModel = pageViewModel,
                Users = pageItems.Except(user.FamilyMembers.Select(fm=>fm.Member)),
                FamilyMembers = user.FamilyMembers.Select(fm=>fm.Member),
                SearchString = search
            };

            ViewData["ReturnUrl"] = $"~/Student/FamilyList";
            return View(viewModel);
        }

        private IQueryable<ApplicationUser> SearchUser(IQueryable<ApplicationUser> source, string searchStr)
        {
            if (!string.IsNullOrWhiteSpace(searchStr))
            {
                searchStr = searchStr.ToLower();
                source = source.Where(u => u.UserName.ToLower().Contains(searchStr));
            }

            return source;
        }


        [HttpPost]
        public async Task<IActionResult> AddFamilyMember(string memberID,string returnUrl=null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            var member = await _context.ApplicationUser.FindAsync(memberID);
            var familyMember = new FamilyMember {Member = member, User = await GetCurrentUserAsync()};
            _context.FamilyMembers.Add(familyMember);
            await _context.SaveChangesAsync();
            return RedirectToLocal(returnUrl);
        }

        [HttpPost]
        public async Task<IActionResult> RemoveFamilyMember(string memberID, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            var member = await _context.ApplicationUser.FindAsync(memberID);
            var familyMember =await _context.FamilyMembers.FirstAsync(fm=>fm.Member==member&&fm.User==GetCurrentUserAsync().Result);
            _context.FamilyMembers.Remove(familyMember);
            await _context.SaveChangesAsync();
            return RedirectToLocal(returnUrl);
        }

        #endregion

        #region Helpers

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

        private Task<ApplicationUser> GetCurrentUserAsync()
        {
            return _userManager.GetUserAsync(HttpContext.User);
        }

        #endregion


    }
}