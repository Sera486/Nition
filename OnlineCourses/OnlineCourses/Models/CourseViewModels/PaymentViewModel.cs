using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineCourses.Models.CourseViewModels
{
    public class PaymentViewModel
    {
        public Course Course { get; set; }
        public ApplicationUser User { get; set; }
    }
}
