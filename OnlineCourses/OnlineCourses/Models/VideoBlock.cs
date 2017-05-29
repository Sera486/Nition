namespace OnlineCourses.Models
{
    public class VideoBlock
    {
        public int ID { get; set; }
        public Lesson Lesson { get; set; }
        public string VideoURL {get; set;}
    }
}
