using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Net.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;

namespace MedOk.Controllers
{
    [Produces("application/json")]
    [Authorize]
    public class ImageController : Controller
    {
        private readonly IHostingEnvironment _appEnvironment;
        private readonly UserManager<ApplicationUser> _userManager;

        public ImageController(IHostingEnvironment appEnvironment, UserManager<ApplicationUser> userManager)
        {
            _appEnvironment = appEnvironment;
            _userManager = userManager;
        }

        // GET: Image/GetAvatar/5
        [HttpGet("image/GetAvatar/{id}",Name ="GetAvatar")]
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
                return Json(new {url = $"{Request.Host}/{user.ImageURL}"});
            }
        }
        
        // POST: Image/SetAvatar
        [HttpPost("image/SetAvatar")]
        public async Task<IActionResult> SetAvatar(IFormFile uploadedFile)
        {
            if (uploadedFile != null)
            {
                try
                {
                    var user = await _userManager.GetUserAsync(HttpContext.User);
                    // avatars folder path
                    string path = "/images/avatars/"+user.Id+Path.GetExtension(uploadedFile.FileName);
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
                        return Json(new {result = true});
                    }
                    else
                    {
                        throw new Exception(result.ToString());
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    return Json(new {result = false});
                }
            }
            else
            {
                return Json(new { result = false });
            }
        }
       
    }
}

