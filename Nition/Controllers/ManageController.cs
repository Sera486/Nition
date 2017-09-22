using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Nition.Data;
using Nition.Models;
using Nition.Models.ManageViewModels;
using Nition.Services;

namespace Nition.Controllers
{
    [Authorize]
    public class ManageController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ApplicationDbContext _context;
        private readonly IHostingEnvironment _appEnvironment;
        private readonly IEmailSender _emailSender;
        private readonly ISmsSender _smsSender;
        private readonly ILogger _logger;

        public ManageController(
          UserManager<ApplicationUser> userManager,
          SignInManager<ApplicationUser> signInManager,
          ApplicationDbContext context,
          IHostingEnvironment appEnvironment,
          IEmailSender emailSender,
          ISmsSender smsSender,
          ILoggerFactory loggerFactory)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _appEnvironment = appEnvironment;
            _emailSender = emailSender;
            _smsSender = smsSender;
            _logger = loggerFactory.CreateLogger<ManageController>();
            _context = context;
        }

        
        public async Task<IActionResult> Index(string id)
        {
            ApplicationUser user = _context.ApplicationUser.Find(id);
            if (await _userManager.IsInRoleAsync(user, RolesData.Lecturer))
            {
                return View("LecturerAccount", _context.ApplicationUser.Include(a => a.CreatedCourses).ThenInclude(s => s.Author).AsNoTracking().First(c => c.Id == id));
            }
            if (await _userManager.IsInRoleAsync(user, RolesData.Student))
            {
                return View("StudentAccount",
                    _context.ApplicationUser.Include(a => a.Subscriptions).ThenInclude(s => s.Course)
                        .ThenInclude(s => s.Author)
                        .Include(u => u.SharingUsers).ThenInclude(fm => fm.User).ThenInclude(u => u.Subscriptions)
                        .ThenInclude(s => s.Course).ThenInclude(c => c.Author).AsNoTracking()
                        .First(c => c.Id == id));
            }
            return View("Error");
        }

        [HttpGet("ViewComponent/UserCoursesList")]
        public IActionResult UserCoursesListViewComponent(string userID,int page)
        {
            return ViewComponent("UserCoursesList", new { userID = userID, page = page });
        }

        [HttpGet]
        public async Task<IActionResult> EditAccountInfo(string id)
        {
            return View(CreateModel(id, ""));
        }

        public EditAccountInfoViewModel CreateModel(string id, string message)
        {
            var user = _context.ApplicationUser.Find(id);
            var model = new EditAccountInfoViewModel()
            {
                Id = user.Id,
                UserName = user.UserName,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                AboutMe = user.AboutMe,
                Facebook = user.Facebook,
                Twitter = user.Twitter,
                Linkedin = user.Linkedin,
                Skype = user.Skype,
                ValidImageUrl = user.ValidImageURL,
                Message = message
            };
            return model;
        }

        [HttpPost]
        public async Task<IActionResult> EditAccountInfoMainData(EditAccountInfoViewModel model)
        {
            string message = "";
            var user = _context.ApplicationUser.Find(model.Id);
            if (!String.IsNullOrEmpty(model.FirstName))
            {
                user.FirstName = model.FirstName;
            }
            else
            {
                message = message + "Поле \"Ім\'я\" не може бути пустим. ";
            }
            if (!String.IsNullOrEmpty(model.LastName))
            {
                user.LastName = model.LastName;
            }
            else
            {
                message = message + "Поле \"Прізвище\" не може бути пустим. ";
            }
            if (model.Email.IndexOf('@') > -1)
            {
                //Validate email format
                string emailRegex = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
                                    @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
                                    @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
                Regex re = new Regex(emailRegex);
                if (re.IsMatch(model.Email))
                {
                    user.Email = model.Email;
                }
                else
                {
                    message = message + "Неправильно введено емейл. ";
                }
            }
            else
            {
                message = message + "Неправильно введено емейл. ";
            }

            if (model.Image != null)
            {
                string path = Path.Combine("images", "courseLogos",
                    Guid.NewGuid() + Path.GetExtension(model.Image.FileName));

                // saving image in wwwroot
                using (var fileStream = new FileStream(Path.Combine(_appEnvironment.WebRootPath, path),
                    FileMode.Create))
                {
                    await model.Image.CopyToAsync(fileStream);
                }
                if(!String.IsNullOrEmpty(_context.ApplicationUser.Find(model.Id).ImageURL)){
                    System.IO.File.Delete(Path.Combine(_appEnvironment.WebRootPath,
                        _context.ApplicationUser.Find(model.Id).ImageURL));
                }
                user.ImageURL = path;
            }

            _context.ApplicationUser.Update(user);

            await _context.SaveChangesAsync();
            return View("EditAccountInfo", CreateModel(model.Id, message));
        }

        [HttpPost]
        public async Task<IActionResult> EditAccountInfoAboutMe(EditAccountInfoViewModel model)
        {
            var user = _context.ApplicationUser.Find(model.Id);
            user.AboutMe = model.AboutMe;

            _context.ApplicationUser.Update(user);

            await _context.SaveChangesAsync();
            return View("EditAccountInfo", CreateModel(model.Id, ""));
        }

        [HttpPost]
        public async Task<IActionResult> EditAccountInfoContacts(EditAccountInfoViewModel model)
        {
            var user = _context.ApplicationUser.Find(model.Id);
            user.Linkedin = model.Linkedin;
            user.Skype = model.Skype;
            user.Twitter = model.Twitter;
            user.Facebook = model.Facebook;

            _context.ApplicationUser.Update(user);

            await _context.SaveChangesAsync();
            return View("EditAccountInfo", CreateModel(model.Id, ""));
        }

        //
        // GET: /Manage/ChangePassword
        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View();
        }

        //
        // POST: /Manage/ChangePassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await GetCurrentUserAsync();
            if (user != null)
            {
                if (model.NewPassword.Equals(model.ConfirmPassword))
                {
                    var result = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
                    if (result.Succeeded)
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        _logger.LogInformation(3, "User changed their password successfully.");
                        return View("EditAccountInfo", CreateModel(user.Id, "Пароль успішно змінено"));
                    }
                }
                else
                {
                    return View("EditAccountInfo", CreateModel(user.Id, "Паролі не співпадають. Спробуйте ще раз"));
                }
            }
            return View("EditAccountInfo", CreateModel(user.Id, "При зміні паролю допущено помилку. Спробуйте ще раз"));
        }

        //
        // GET: /Manage/SetPassword
        [HttpGet]
        public IActionResult SetPassword()
        {
            return View();
        }

        //
        // POST: /Manage/SetPassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SetPassword(SetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await GetCurrentUserAsync();
            if (user != null)
            {
                var result = await _userManager.AddPasswordAsync(user, model.NewPassword);
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction(nameof(Index), new { Message = ManageMessageId.SetPasswordSuccess });
                }
                AddErrors(result);
                return View(model);
            }
            return RedirectToAction(nameof(Index), new { Message = ManageMessageId.Error });
        }


        #region Helpers

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        public enum ManageMessageId
        {
            AddPhoneSuccess,
            AddLoginSuccess,
            ChangePasswordSuccess,
            SetTwoFactorSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
            RemovePhoneSuccess,
            Error
        }

        private Task<ApplicationUser> GetCurrentUserAsync()
        {
            return _userManager.GetUserAsync(HttpContext.User);
        }

        #endregion
    }
}
