using System;

namespace OnlineCourses.Models.CourseViewModels
{
    public class CourseInfoViewModel
    {
        public Course Course { get; set; }
        public Boolean Paid { get; set; }
        public Boolean IsAuthor { get; set; }
        public Boolean IsStudent { get; set; }
    }
}
