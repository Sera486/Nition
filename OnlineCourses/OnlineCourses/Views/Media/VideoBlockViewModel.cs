using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace OnlineCourses.Views.Media
{
    public class VideoBlockViewModel
    {
        public int LessonID { get; set; }
        public IFormFile UploadedFile { get; set; }
    }
}
