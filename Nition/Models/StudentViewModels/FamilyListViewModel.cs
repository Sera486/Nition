using System.Collections.Generic;

namespace Nition.Models.StudentViewModels
{
    public class FamilyListViewModel
    {
        public IEnumerable<ApplicationUser> FamilyMembers { get; set; }
        public IEnumerable<ApplicationUser> Users { get; set; }
        public PageViewModel PageViewModel { get; set; }
        public string SearchString { get; set; }
    }
}
