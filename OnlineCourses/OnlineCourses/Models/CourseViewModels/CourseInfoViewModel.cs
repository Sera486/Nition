using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
