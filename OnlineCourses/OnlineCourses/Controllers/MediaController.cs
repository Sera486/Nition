using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using OnlineCourses.Data;
using OnlineCourses.Models;
using OnlineCourses.Views.Media;

namespace OnlineCourses.Controllers
{
    [Authorize]
    public class ImageController : Controller
    {
        private readonly IHostingEnvironment _appEnvironment;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        public ImageController(IHostingEnvironment appEnvironment, UserManager<ApplicationUser> userManager,ApplicationDbContext context)
        {
            _appEnvironment = appEnvironment;
            _userManager = userManager;
            _context = context;
        }

        // GET: mmedia/GetAvatar/5
        [HttpGet("media/GetAvatar/{id}",Name ="GetAvatar")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAvatar(string id)
        {
            var user = await  _userManager.FindByIdAsync(id);
            if (string.IsNullOrWhiteSpace(user?.ImageURL))
            {
                return Json(new { result=false });
            }
            else
            {
                return Json(new {result = true, url = $"{Request.Host}/{user.ValidImageURL}"});
            }
        }

        // POST: media/SetAvatar
        [HttpPost("media/SetAvatar")]
        public async Task<IActionResult> SetAvatar(IFormFile uploadedFile, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (uploadedFile != null)
            {
                try
                {
                    var user = await _userManager.GetUserAsync(HttpContext.User);
                    
                    // avatars folder path
                    string path = "images/avatars/"+user.Id+Path.GetExtension(uploadedFile.FileName);
                    
                    // saving image in avatars folder in wwwroot
                    using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                    {
                        await uploadedFile.CopyToAsync(fileStream);
                    }

                    //updating user
                    user.ImageURL = path;
                    var result = await _userManager.UpdateAsync(user);
                    
                    //result
                    if (result.Succeeded)
                    {
                        return RedirectToLocal(returnUrl);
                    }
                    else
                    {
                        throw new Exception(result.ToString());
                    }
                }
                catch (Exception e)
                {
                    return Json(new {result = false});
                }
            }
            else
            {
                return Json(new { result = false });
            }
        }

        [HttpPost("media/SetCourseImage")]
        public async Task<IActionResult> SetCourseImage(CourseImageViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (model.UploadedFile != null)
            {
                try
                {
                    var course = _context.Courses.Find(model.CourseID);

                    // avatars folder path
                    string path = $"images/courseLogos/{Guid.NewGuid()}{Path.GetExtension(model.UploadedFile.FileName)}";

                    // saving image in wwwroot
                    using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                    {
                        await model.UploadedFile.CopyToAsync(fileStream);
                    }

                    //updating course
                    course.ImageURL = path;
                    _context.SaveChanges();

                    return RedirectToLocal(returnUrl);
                }
                catch (Exception e)
                {
                    return Json(new { result = false });
                }
            }
            else
            {
                return Json(new { result = false });
            }
        }


        // POST: media/addVideoBlock
        [HttpPost("media/AddVideoBlock")]
        public async Task<IActionResult> AddVideoBlock(VideoBlockViewModel model,string returnUrl=null)
        {
            if (model.UploadedFile != null)
            {
                try
                {
                    var lesson = await _context.Lessons.FindAsync(model.LessonID);
                    
                    // attachment folder path
                    string path = $"attachments/videos/{Guid.NewGuid()},{Path.GetExtension(model.UploadedFile.FileName)}";
                    
                    // saving image in avatars folder in wwwroot
                    using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                    {
                        await model.UploadedFile.CopyToAsync(fileStream);
                    }
                    
                    //updating table
                    var VideoBlock = new VideoBlock
                    {
                        VideoURL = path,
                        Lesson = lesson
                    };
                    _context.VideoBlocks.Add(VideoBlock);
                    _context.SaveChanges();

                    //result
                    return RedirectToLocal(returnUrl);
                }
                catch (Exception e)
                {
                    return Json(new {result = false});
                }
            }
            else
            {
                return Json(new {result = false});
            }
        }
       
        /*
        [HttpPost("media/AddAttachment")]
        public async Task<IActionResult> GetAttachments(int recordID)
        {
            var record = _context.MedicalRecords.Find(recordID);
            var attachments = _context.Attachments.Where(e => e.MedicalRecord ==record ).ToList();
            if (attachments==null)
            {
                return Json(new { result = false });
            }
            else
            {
                return Json(new {result =true, attachments=attachments.Select(c=>new {c.AttachmentType,c.AttachmentURL})});
            }
        }
        */
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

