using Microsoft.AspNetCore.Http;

namespace OnlineCourses.Models.MediaViewModels
{
    public class CourseImageViewModel
    {
        public int CourseID { get; set; }
        public IFormFile UploadedFile {get; set; }
    }
}
