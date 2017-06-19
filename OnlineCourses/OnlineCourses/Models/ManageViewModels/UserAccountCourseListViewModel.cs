using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineCourses.Models.ManageViewModels
{
    public class UserAccountCourseListViewModel
    {
        public PageViewModel PageViewModel { get; set; }
        public List<Course> Courses { get; set; }
        public string UserID { get; set; }
    }
}
