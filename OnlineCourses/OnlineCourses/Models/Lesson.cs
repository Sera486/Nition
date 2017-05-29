using System.Collections.Generic;

namespace OnlineCourses.Models
{
    public class Lesson
    {
        public int ID { get; set; }
        public int Order { get; set; }
        public Course Course { get; set; }
        public string Title {get; set;}
        public string Description {get; set;}
        public List<Comment> Comments { get; set; }
        public List<TextBlock> TextBlocks {get; set;}
        public List<VideoBlock> VideoBlocks { get; set; }
    }
}
