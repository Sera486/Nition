using System;

namespace OnlineCourses.Models
{
    public class Comment
    {
        public int ID { get; set; }
        public ApplicationUser User {get; set;}
        public string Text{get;set;}
        public DateTime Date{get;set;}
    }
}
