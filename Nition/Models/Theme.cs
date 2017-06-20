using System.Collections.Generic;

namespace Nition.Models
{
    public class Theme
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string ImageURL { get; set; }
        public List<CourseTheme> CourseThemes { get; set; }

        public string ValidImageURL => string.IsNullOrWhiteSpace(ImageURL) ? "img/no_image.png" : ImageURL;
    }
}
