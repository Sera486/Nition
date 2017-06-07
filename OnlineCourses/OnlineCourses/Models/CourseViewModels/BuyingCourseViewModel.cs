using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineCourses.Models.CourseViewModels
{
    public class BuyingCourseViewModel
    {
        public Course Course { get; set; }
        public List<Lesson> Lessons { get; set; }
        public ApplicationUser Author { get; set; }
        public Boolean Paid { get; set; }
        public Boolean IsAuthor { get; set; }
    }
}
