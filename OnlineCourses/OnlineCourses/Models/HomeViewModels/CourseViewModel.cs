using System.Collections;
using System.Collections.Generic;

namespace OnlineCourses.Models.HomeViewModels
{
    public class CourseViewModel:IEnumerable<Course>
    {
        public IEnumerable<Course> Courses { get; set; }
        public PageViewModel PageViewModel { get; set; }
        public string SearchString { get; set; }
        public bool SearchInDescription { get; set; }
        public int ThemeID { get; set; }

        public IEnumerator<Course> GetEnumerator()
        {
            return Courses.GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
