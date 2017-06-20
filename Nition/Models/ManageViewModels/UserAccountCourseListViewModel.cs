using System.Collections.Generic;

namespace Nition.Models.ManageViewModels
{
    public class UserAccountCourseListViewModel
    {
        public PageViewModel PageViewModel { get; set; }
        public List<Course> Courses { get; set; }
        public string UserID { get; set; }
    }
}
