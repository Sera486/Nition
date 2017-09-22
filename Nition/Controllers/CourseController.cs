using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Nition.Data;
using Nition.Models;
using Nition.Models.CourseViewModels;
using Nition.Models.Enums;
using Nition.Models.LecturerViewModels;

namespace Nition.Controllers
{
    public class CourseController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;
        private int popularCoursesCount = 12;

        public CourseController(
            UserManager<ApplicationUser> userManager,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int page=1, string search="",int theme=0)
        {
            var pageSize = 5;
            //selecting courses with all info
            IQueryable<Course> source = _context.Courses
                .Include(course => course.Author)
                .Include(course => course.CourseThemes)
                .Where(c => c.PublishStatus == PublishStatus.Published);
            
            //searching through courses
            var allItems =await SearchCourse( source, search, theme).ToListAsync();
            
            //splitting into pages
            var count = allItems.Count;
            var pageItems = allItems.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            var pageViewModel = new PageViewModel(count, page, pageSize);

            var viewModel = new CourseListViewModel
            {
                PageViewModel = pageViewModel,
                Courses = pageItems,
                SearchString = search,
                ThemeID = theme
            };

            //filling themes selector
            if (_context.Themes.Count() != 0)
            {
                var themes = _context.Themes
                    .Select(t => new SelectListItem
                    {
                        Text = t.Name,
                        Value = t.ID.ToString(),
                        Selected = t.CourseThemes.Exists(th => th.ThemeID == theme)
                    }).ToList();
                ViewBag.Themes = themes;
            }
            return View(viewModel);
        }

        [HttpGet("PopularCourses")]
        public async Task<IActionResult> PopularCourses(int page=1)
        {
            var pageSize = 5;
            //selecting courses with all info
            IQueryable<Course> source = _context.Courses
                .Include(course => course.Author).Include(course => course.Subscriptions)
                .Where(c => c.PublishStatus == PublishStatus.Published)
                .OrderByDescending(c => c).ThenByDescending(c => c .Subscriptions.Count);
            
            if (_context.Courses.Count() < popularCoursesCount)
            {
                popularCoursesCount = _context.Courses.Count();
            }
            
            source = source.Take(popularCoursesCount);
            var pageItems = source.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            var pageViewModel = new PageViewModel(popularCoursesCount, page, pageSize);

            var viewModel = new PopularCoursesViewModel()
            {
                PageViewModel = pageViewModel,
                Courses = pageItems
            };
            
            return View(viewModel);
        }

        public static IQueryable<Course> SearchCourse(IQueryable<Course> source, string searchStr, int themeID=0, ApplicationUser author=null)
        {
            if (!string.IsNullOrWhiteSpace(searchStr))
            {
                searchStr = searchStr.ToLower();
                source = source.Where(c => c.Title.ToLower().Contains(searchStr) ||
                                           c.Description.ToLower().Contains(searchStr));
            }
            if (themeID != 0)
            {
                source = source.Where(c => c.CourseThemes.Any(e => e.ThemeID == themeID));
            }
            if (author != null)
            {
                source = source.Where(c => c.Author == author);
            }
            return source;
        }

        [HttpGet("Lesson/{LessonId}")]
        public IActionResult Lesson(int LessonId)
        {
            var lesson = _context.Lessons.Include(l => l.TextBlocks).Include(l => l.VideoBlocks).Include(l => l.Course).ThenInclude(l => l.Lessons).AsNoTracking().FirstOrDefault(c=>c.ID==LessonId);
            List<InfoBlock> list= lesson.TextBlocks.Cast<InfoBlock>().ToList();
            list.AddRange(lesson.VideoBlocks);
            var viewModel = new LessonViewModel()
            {
                Lesson = lesson,
                InfoBlocks = list.OrderBy(c => c.Order).ToList()
            };
            return View(viewModel);
        }
        
        public IActionResult Payment(int courseID)
        {
            return View(_context.Courses.Include(c => c.Author).First(c => c.ID == courseID));
        }

        [HttpPost]
        public async Task<IActionResult> Buy(int courseID)
        {
                var sub = new Subscription()
                {
                    Course = _context.Courses.Find(courseID),
                    User = GetCurrentUserAsync().Result,
                    SubscriptionDate = DateTime.Today
                };
                _context.Subscriptions.Add(sub);
                await _context.SaveChangesAsync();
            var viewModel = new CourseInfoViewModel()
            {
                Course = await _context.Courses.Include(c => c.Author)
                    .Include(c => c.Lessons)
                    .Include(c => c.Subscriptions)
                    .Include(c=>c.Comments).ThenInclude(c=>c.User).Where(c => c.ID == courseID).FirstOrDefaultAsync(),
                Paid = true
            };
            return View("CourseInfo", viewModel);
        }

        [HttpGet("Course/{id}")]
        public async Task<IActionResult> CourseInfo(int ID)
        {
            
            var user = await GetCurrentUserAsync();
            var source = _context.Courses.Include(c => c.Author)
                .Include(c => c.Lessons)
                .Include(c => c.Subscriptions)
                .Include(c=>c.Comments).ThenInclude(c=>c.User);

            Course course = await source.Where(c => c.ID == ID).FirstOrDefaultAsync();

            if (user != null)
            {
                if (course.Author == user)
                {
                    RedirectToAction(nameof(LecturerController.CourseEditor), "Lecturer");
                }
                user = _context.ApplicationUser
                    .Include(c => c.Subscriptions).ThenInclude(s=>s.Course)
                    .Include(u=>u.SharingUsers).ThenInclude(fm=>fm.User).ThenInclude(u=>u.Subscriptions).ThenInclude(s=>s.Course).AsNoTracking()
                    .First(c => c.Id == user.Id);
                var validSubscriptions = user.ValidSubscriptions();
                if (validSubscriptions.Count(c=>c.Course.ID==course.ID) == 0)
                {
                    var viewModel = new CourseInfoViewModel()
                    {
                        Course = course,
                        Paid = false
                    };
                    ViewData["ReturnUrl"] = $"Course/{course.ID}";
                    return View(viewModel);
                }
                else
                {
                    var viewModel = new CourseInfoViewModel()
                    {
                        Course = course,
                        Paid = true
                    };
                    ViewData["ReturnUrl"] = $"Course/{course.ID}";
                    return View(viewModel);
                }
            }
            else
            {
                var viewModel = new CourseInfoViewModel()
                {
                    Course = course,
                    Paid = false
                };
                ViewData["ReturnUrl"] = $"Course/{course.ID}";
                return View(viewModel);
            }
        }

        [HttpGet("ViewComponent/CommentList/{courseID}")]
        public IActionResult CommentListViewComponent(int courseID)
        {
            return ViewComponent("CommentList", courseID );
        }

        [HttpGet("ViewComponent/LessonList/{courseID}")]
        public IActionResult LessonsListViewComponent(int courseID,bool isPaid)
        {
            return ViewComponent("LessonList", new {courseID , isPaid});
        }

        [HttpPost]
        public async Task AddComment(int courseID, string commentText)
        {
            var course = _context.Courses.Include(c => c.Comments).First(c=>c.ID==courseID);
            if (!string.IsNullOrWhiteSpace(commentText))
            {
                var user = await GetCurrentUserAsync();
                course.Comments.Add(new Comment {Date = DateTime.Now, Text = commentText, User = user});
                _context.Update(course);
                await _context.SaveChangesAsync();
            }
        }

        [HttpPost]
        public async Task DeleteComment(int commentID)
        {
            //TODO: никакой проверки прав на удаление, опасность
            var comment =new Comment{ID = commentID};

            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();
            
        }

        [HttpPost]
        public async Task<IActionResult> MarkComment(int commentID, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            var comment = await _context.Comments.FindAsync(commentID);
            if (comment != null)
            {
                comment.Status=CommentStatus.Offensive;
                _context.Comments.Update(comment);
                await _context.SaveChangesAsync();
            }

            return RedirectToLocal(returnUrl);
        }

        [HttpPost]
        public async Task<IActionResult> UnmarkComment(int commentID, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            var comment = await _context.Comments.FindAsync(commentID);
            if (comment != null)
            {
                comment.Status = CommentStatus.Normal;
                _context.Comments.Update(comment);
                await _context.SaveChangesAsync();
            }

            return RedirectToLocal(returnUrl);
        }

        private Task<ApplicationUser> GetCurrentUserAsync()
        {
            return _userManager.GetUserAsync(HttpContext.User);
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
