using System.Collections.Generic;

namespace Nition.Models.LecturerViewModels
{
    public class LessonViewModel
    {
        public Lesson Lesson { get; set; }
        public List<InfoBlock> InfoBlocks { get; set; }
    }
}
