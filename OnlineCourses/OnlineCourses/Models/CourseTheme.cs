using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineCourses.Models
{
    public class CourseTheme
    {
        public int CourseID { get; set; }
        public Course Course { get; set; }
        public int ThemeID { get; set; }
        public Theme Theme { get; set; }
    }
}
