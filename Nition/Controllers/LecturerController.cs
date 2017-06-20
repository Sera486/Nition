using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
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
    [Authorize(Roles = RolesData.Lecturer)]
    public class LecturerController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly IHostingEnvironment _appEnvironment;

        public LecturerController(ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            IHostingEnvironment appEnvironment)
        {
            _context = context;
            _userManager = userManager;
            _appEnvironment = appEnvironment;
        }

        [HttpGet]
        public async Task<IActionResult> CourseList(int page = 1, string search = "")
        {
            var pageSize = 5;
            //selecting courses with all info
            IQueryable<Course> source = _context.Courses
                .Include(course => course.Author)
                .Include(course => course.CourseThemes);

            //searching through courses
            var allItems = await CourseController.SearchCourse(source, search,0,await GetCurrentUser()).ToListAsync();

            //splitting into pages
            var count = allItems.Count;
            var pageItems = allItems.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            var pageViewModel = new PageViewModel(count, page, pageSize);

            var viewModel = new CourseListViewModel
            {
                PageViewModel = pageViewModel,
                Courses = pageItems,
                SearchString = search
            };
            
            return View(viewModel);
        }

        [HttpGet]
        public IActionResult CreateCourse()
        {
            ViewBag.Themes = _context.Themes.Select(t => new SelectListItem {Text = t.Name, Value = t.ID.ToString()}).ToList();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateCourse(CreateCourseViewModel vm)
        {
            try
            {
                //image upload
                string imageUrl = null;
                if (vm.Image != null)
                {
                    string path = Path.Combine("images","courseLogos",Guid.NewGuid()+Path.GetExtension(vm.Image.FileName));

                    // saving image in wwwroot
                    using (var fileStream = new FileStream(Path.Combine(_appEnvironment.WebRootPath, path), FileMode.Create))
                    {
                        await vm.Image.CopyToAsync(fileStream);
                    }
                    imageUrl = path;
                }
                
                var course = new Course()
                {
                    Title = vm.Title,
                    Description = vm.Description,
                    Author = GetCurrentUser().Result,
                    CreationDate = DateTime.Today,
                    ModificationDate = DateTime.Today,
                    CourseThemes = vm.ThemeID == 0 ?null:new List<CourseTheme>{new CourseTheme{ThemeID = vm.ThemeID}},
                    Estimate = vm.Estimate,
                    Price = vm.Price,
                    ImageURL = imageUrl
                };

                _context.Courses.Add(course);
                _context.SaveChanges();

                return RedirectToLocal($"~/Course/{course.ID}");
            }
            catch (Exception e)
            {
                ViewData["Message"]="Cталася помилка, спробуйте, будь ласка, ще раз.";
                return View("Error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteCourse(int courseID)
        {
            try
            {
                _context.Courses.Remove(_context.Courses.Find(courseID));

                await _context.SaveChangesAsync();
                return Json(new { result = true });
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return Json(new { result = false });
            }
        }

		 public async Task<IActionResult> PublishCourse(int ID, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            var course = await _context.Courses.FindAsync(ID);
            course.PublishStatus = PublishStatus.Proccesing;
            _context.Courses.Update(course);
            await _context.SaveChangesAsync();
            return RedirectToLocal(returnUrl);
        }

        [HttpPost]
        public async Task<IActionResult> HideCourse(int ID, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            var course = await _context.Courses.FindAsync(ID);
            course.PublishStatus = PublishStatus.Hidden;
            _context.Courses.Update(course);
            await _context.SaveChangesAsync();
            return RedirectToLocal(returnUrl);
        }

        #region Lesson editing

        [HttpGet("LessonEditor/{LessonId}")]
        public IActionResult LessonEditor(int LessonId)
        {
            var lesson = _context.Lessons.Include(l => l.TextBlocks).Include(l => l.VideoBlocks).Include(l => l.Course).FirstOrDefault(c => c.ID == LessonId);
            List<InfoBlock> list = lesson.TextBlocks.Cast<InfoBlock>().ToList();
            list.AddRange(lesson.VideoBlocks);
            var viewModel = new LessonViewModel()
            {
                Lesson = lesson,
                InfoBlocks = list.OrderBy(c => c.Order).ToList()
            };
            ViewData["ReturnUrl"] = $"~/LessonEditor/{lesson.ID}";
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> AddLesson(int courseID, string title, string description, string returnUrl = null)
        {
            try
            {
                ViewData["ReturnUrl"] = returnUrl;
                Course course = _context.Courses.Include(c => c.Lessons).First(c => c.ID == courseID);
                _context.Lessons.Add(new Lesson()
                {
                    Order = course.Lessons.Count + 1,
                    Course = course,
                    Title = title,
                    Description = description
                });

                await _context.SaveChangesAsync();
                return RedirectToLocal(returnUrl);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return Json(new { result = false });
            }
        }

        public async Task<IActionResult> DeleteLesson(int lessonID, string returnUrl)
        {
            try
            {
                ViewData["ReturnUrl"] = returnUrl;
                _context.Lessons.Remove(new Lesson{ID = lessonID});

                await _context.SaveChangesAsync();
                return RedirectToLocal(returnUrl);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return Json(new { result = false });
            }
        }


        [HttpPost]
        public async Task<IActionResult> AddTextBlock(int lessonId, string text, string returnUrl)
        {
            try
            {
                ViewData["ReturnUrl"] = returnUrl;
                _context.TextBlocks.Add(new TextBlock()
                {
                    Lesson = _context.Lessons.Find(lessonId),
                    Text = text,
                    Order = _context.TextBlocks.Max(c => c.Order) > _context.VideoBlocks.Max(c => c.Order) ? _context.TextBlocks.Max(c => c.Order) + 1 : _context.VideoBlocks.Max(c => c.Order) + 1
                });
                await _context.SaveChangesAsync();
                return RedirectToLocal(returnUrl);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return Json(new { result = false });
            }
        }

        [HttpPost]
        public async Task<IActionResult> EditTextBlock(int textBlockId, string text, string returnUrl)
        {
            try
            {
                ViewData["ReturnUrl"] = returnUrl;
                var textBlock = _context.TextBlocks.Find(textBlockId);
                textBlock.Text = text;
                _context.TextBlocks.Update(textBlock);

                await _context.SaveChangesAsync();
                return RedirectToLocal(returnUrl);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return Json(new { result = false });
            }
        }

        [HttpPost]
        public async Task<IActionResult> EditDescription(int lessonId, string description, string returnUrl)
        {
            try
            {
                ViewData["ReturnUrl"] = returnUrl;
                var lesson = _context.Lessons.Find(lessonId);
                lesson.Description = description;
                _context.Lessons.Update(lesson);

                await _context.SaveChangesAsync();
                return RedirectToLocal(returnUrl);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return Json(new { result = false });
            }
        }

        [HttpPost]
        public async Task<IActionResult> EditTitle(int lessonId, string title, string returnUrl)
        {
            try
            {
                ViewData["ReturnUrl"] = returnUrl;
                var lesson = _context.Lessons.Find(lessonId);
                lesson.Title = title;
                _context.Lessons.Update(lesson);

                await _context.SaveChangesAsync();
                return RedirectToLocal(returnUrl);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return Json(new { result = false });
            }
        }

        public async Task<IActionResult> DeleteTextBlock(int textBlockID, string returnUrl)
        {
            try
            {
                ViewData["ReturnUrl"] = returnUrl;
                _context.TextBlocks.Remove(_context.TextBlocks.Find(textBlockID));

                await _context.SaveChangesAsync();
                return RedirectToLocal(returnUrl);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return Json(new { result = false });
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddVideoBlock(VideoBlockViewModel model, string returnUrl)
        {
            try
            {
                ViewData["ReturnUrl"] = returnUrl;
                // attachment folder path
                string path = Path.Combine("videos", "lessonBlocks", Guid.NewGuid() + Path.GetExtension(model.UploadedFile.FileName));

                // saving video in avatars folder in wwwroot
                using (var fileStream = new FileStream(Path.Combine(_appEnvironment.WebRootPath, path), FileMode.Create))
                {
                    await model.UploadedFile.CopyToAsync(fileStream);
                }

                //updating table
                var VideoBlock = new VideoBlock
                {
                    VideoURL = path,
                    Lesson = _context.Lessons.Find(model.LessonID),
                    Order = _context.TextBlocks.Max(c => c.Order) > _context.VideoBlocks.Max(c => c.Order)
                        ? _context.TextBlocks.Max(c => c.Order) + 1
                        : _context.VideoBlocks.Max(c => c.Order) + 1
                };
                _context.VideoBlocks.Add(VideoBlock);
                _context.SaveChanges();

                return RedirectToLocal(returnUrl);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return Json(new { result = false });
            }
        }

        public async Task<IActionResult> DeleteVideoBlock(int videoBlockID, string returnUrl)
        {
            try
            {
                ViewData["ReturnUrl"] = returnUrl;
                System.IO.File.Delete(Path.Combine(_appEnvironment.WebRootPath, _context.VideoBlocks.Find(videoBlockID).VideoURL));
                _context.VideoBlocks.Remove(_context.VideoBlocks.Find(videoBlockID));

                await _context.SaveChangesAsync();
                return RedirectToLocal(returnUrl);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return Json(new { result = false });
            }
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

        private async Task<ApplicationUser> GetCurrentUser()
        {
            return await _userManager.GetUserAsync(HttpContext.User);
        }
        #endregion

    }
}