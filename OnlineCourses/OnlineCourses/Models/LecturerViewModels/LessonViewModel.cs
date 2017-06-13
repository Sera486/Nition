using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineCourses.Models.LecturerViewModels
{
    public class LessonViewModel
    {
        public Lesson Lesson { get; set; }
        public List<InfoBlock> InfoBlocks { get; set; }
    }
}
