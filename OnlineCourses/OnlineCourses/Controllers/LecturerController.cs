using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineCourses.Data;
using OnlineCourses.Models;
using OnlineCourses.Models.Enums;
using OnlineCourses.Models.LecturerViewModels;

namespace OnlineCourses.Controllers
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

        private async Task<ApplicationUser> GetCurrentUser()
        {
            return await _userManager.GetUserAsync(HttpContext.User);
        }

        [HttpGet]
        public IActionResult CreateCourse()
        {
            return View();
        }
        
        // POST: api/Lecturer
        [HttpPost]
        public async Task<IActionResult> CreateCourse(CreateCourseViewModel createCourseViewModel)
        {
            try
            {
                //image upload
                string imageUrl = null;
                if (createCourseViewModel.Image != null)
                {
                    string path = Path.Combine("images","courseLogos",Guid.NewGuid()+Path.GetExtension(createCourseViewModel.Image.FileName));

                    // saving image in wwwroot
                    using (var fileStream = new FileStream(Path.Combine(_appEnvironment.WebRootPath, path), FileMode.Create))
                    {
                        await createCourseViewModel.Image.CopyToAsync(fileStream);
                    }
                    imageUrl = path;
                }

                var course = new Course()
                {
                    Title = createCourseViewModel.Title,
                    Description = createCourseViewModel.Description,
                    Author = GetCurrentUser().Result,
                    CreationDate = DateTime.Today,
                    ModificationDate = DateTime.Today,
                    Estimate = createCourseViewModel.Estimate,
                    Price = createCourseViewModel.Price,
                    ImageURL = imageUrl
                };
                _context.Courses.Add(course);
                await _context.SaveChangesAsync();
                return View("CourseEditor");
            }
            catch (Exception e)
            {
                return View("Error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteCourse([FromBody] int courseID)
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
        
        [HttpGet("LessonEditor/{LessonId}")]
        public IActionResult LessonEditor(int LessonId)
        {
            var lesson = _context.Lessons.Include(l => l.TextBlocks).Include(l => l.VideoBlocks).FirstOrDefault(c=>c.ID==LessonId);
            List<InfoBlock> list= lesson.TextBlocks.Cast<InfoBlock>().ToList();
            list.AddRange(lesson.VideoBlocks);
            var viewModel = new LessonViewModel()
            {
                Lesson = lesson,
                InfoBlocks = list.OrderBy(c => c.Order).ToList()
            };
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> AddLesson(int courseID, string title, string description,  string returnUrl = null)
        {
            try
            {
                ViewData["ReturnUrl"] = returnUrl;
                Course course = _context.Courses.Include(c=>c.Lessons).First(c=>c.ID==courseID);
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
        
        public async Task<IActionResult> DeleteLesson(int lessonID,  string returnUrl)
        {
            try
            {
                ViewData["ReturnUrl"] = returnUrl;
                _context.Lessons.Remove(_context.Lessons.Find(lessonID));

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
                    Order = _context.TextBlocks.Max(c => c.Order) > _context.VideoBlocks.Max(c => c.Order) ? _context.TextBlocks.Max(c => c.Order)+1 : _context.VideoBlocks.Max(c => c.Order)+1
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
                string path = Path.Combine("videos","lessonBlocks",Guid.NewGuid()+Path.GetExtension(model.UploadedFile.FileName));

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
                return Json(new {result = false});
            }
        }

        public async Task<IActionResult> DeleteVideoBlock(int videoBlockID, string returnUrl)
        {
            try
            {
                ViewData["ReturnUrl"] = returnUrl;
                System.IO.File.Delete(Path.Combine(_appEnvironment.WebRootPath,_context.VideoBlocks.Find(videoBlockID).VideoURL));
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

        #endregion

    }
}