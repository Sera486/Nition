using System.Collections;
using System.Collections.Generic;

namespace OnlineCourses.Models.AdminViewModels
{
    public class UserListViewModel:IEnumerable<ApplicationUser>
    {
        public IEnumerable<ApplicationUser> Users { get; set; }
        public PageViewModel PageViewModel { get; set; }
        public string SearchString { get; set; }
        public string Role { get; set; }

        public IEnumerator<ApplicationUser> GetEnumerator()
        {
            return Users.GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
