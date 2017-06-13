using Microsoft.AspNetCore.Http;

namespace OnlineCourses.Models.LecturerViewModels
{
    public class VideoBlockViewModel
    {
        public int LessonID { get; set; }
        public IFormFile UploadedFile { get; set; }
    }
}
