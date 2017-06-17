using System.Collections.Generic;

namespace OnlineCourses.Models
{
    public class Theme
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public List<CourseTheme> CourseThemes { get; set; }
    }
}
