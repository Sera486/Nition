namespace Nition.Models
{
    public class CourseTheme
    {
        public int CourseID { get; set; }
        public Course Course { get; set; }
        public int ThemeID { get; set; }
        public Theme Theme { get; set; }
    }
}
