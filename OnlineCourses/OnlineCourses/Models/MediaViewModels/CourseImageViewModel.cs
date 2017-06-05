using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace OnlineCourses.Views.Media
{
    public class CourseImageViewModel
    {
        public int CourseID { get; set; }
        public IFormFile UploadedFile {get; set; }
    }
}
