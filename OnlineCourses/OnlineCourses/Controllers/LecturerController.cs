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

        [HttpGet]
        public IActionResult LessonEditor(int LessonId)
        {
            var lesson = _context.Lessons.Include(l => l.TextBlocks).Include(l => l.VideoBlocks)
                .Where(l => l.ID == LessonId).ToList()[0];
            List<InfoBlock> list=new List<InfoBlock>();
            foreach (var text in lesson.TextBlocks)
            {
                list.Add(text);
            }
            foreach (var video in lesson.VideoBlocks)
            {
                list.Add(video);
            }
            var viewModel = new LessonEditorViewModel()
            {
                Lesson = lesson,
                InfoBlocks = list.OrderBy(c => c.Order).ToList()
            };
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> AddLesson(int courseID, string title, string description)
        {
            try
            {
                Course course = _context.Courses.Find(courseID);
                _context.Lessons.Add(new Lesson()
                {
                    Order = _context.Lessons.Where(c => c.Course == course).Max(c => c.Order) + 1,
                    Course = course,
                    Title = title,
                    Description = description
                });

                await _context.SaveChangesAsync();
                return Json(new { result = true });
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return Json(new { result = false });
            }
        }
        
        public async Task<IActionResult> DeleteLesson([FromBody] int lessonID)
        {
            try
            {
                _context.Lessons.Remove(_context.Lessons.Find(lessonID));

                await _context.SaveChangesAsync();
                return Json(new { result = true });
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return Json(new { result = false });
            }
        }

        
        [HttpPost]
        public async Task<IActionResult> AddTextBlock(int lessonId, string text)
        {
            try
            {
                _context.TextBlocks.Add(new TextBlock()
                {
                    Lesson = _context.Lessons.Find(lessonId),
                    Text = text,
                    Order = _context.TextBlocks.Max(c => c.Order) > _context.VideoBlocks.Max(c => c.Order) ? _context.TextBlocks.Max(c => c.Order)+1 : _context.VideoBlocks.Max(c => c.Order)+1
                });
                await _context.SaveChangesAsync();
                return Json(new { result = true });
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return Json(new { result = false });
            }
        }

        [HttpPost]
        public async Task<IActionResult> EditTextBlock(int textBlockId, string text)
        {
            try
            {
                var textBlock = _context.TextBlocks.Find(textBlockId);
                textBlock.Text = text;
                _context.TextBlocks.Update(textBlock);

                await _context.SaveChangesAsync();
                return Json(new { result = true });
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return Json(new { result = false });
            }
        }


        public async Task<IActionResult> DeleteTextBlock(int textBlockID)
        {
            try
            {
                _context.TextBlocks.Remove(_context.TextBlocks.Find(textBlockID));

                await _context.SaveChangesAsync();
                return Json(new { result = true });
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return Json(new { result = false });
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddVideoBlock(VideoBlockViewModel model, string returnUrl = null)
        {
            try
            {
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

                return Json(new {result = true});
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return Json(new {result = false});
            }
        }

        public async Task<IActionResult> DeleteVideoBlock(int videoBlockID)
        {
            try
            {
                System.IO.File.Delete(Path.Combine(_appEnvironment.WebRootPath,_context.VideoBlocks.Find(videoBlockID).VideoURL));
                _context.VideoBlocks.Remove(_context.VideoBlocks.Find(videoBlockID));

                await _context.SaveChangesAsync();
                return Json(new { result = true });
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return Json(new { result = false });
            }
        }

        private bool ApplicationUserExists(string id)
        {
            return _context.ApplicationUser.Any(e => e.Id == id);
        }
    }
}