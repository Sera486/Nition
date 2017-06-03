using System;
using System.Collections.Generic;

namespace OnlineCourses.Models
{
    public class Course
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public ApplicationUser Author { get; set; }
        public List<Subscription> Subscriptions { get; set; }
        public List<Lesson> Lessons { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime? ModificationDate {get;set; }
        public string Description { get; set; }
        public string ImageURL { get; set; }
        public double Price { get; set; }
        public List<CourseTheme> CourseThemes { get; set; }
        #region Properties
        public string ValidImageURL => string.IsNullOrWhiteSpace(ImageURL) ? "img/no_image.png" : ImageURL;
        #endregion
    }
}