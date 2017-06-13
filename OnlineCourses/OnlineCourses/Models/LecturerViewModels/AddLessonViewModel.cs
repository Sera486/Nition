using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineCourses.Models.LecturerViewModels
{
    public class AddLessonViewModel
    {
        public Course Course { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }
}
