using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace OnlineCourses.Models.ManageViewModels
{
    public class EditAccountInfoViewModel
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string FirstName {get; set;}
        public string LastName {get; set;}
        public string Email { get; set; }
        public string AboutMe { get; set; }
        public string Facebook { get; set; }
        public string Twitter { get; set; }
        public string Linkedin { get; set; }
        public string Skype { get; set; }
        public IFormFile Image{get; set;}
        public string ValidImageUrl { get; set; }
        public string Message { get; set; }
    }
}
