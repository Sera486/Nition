using System;
using OnlineCourses.Models.Enums;

namespace OnlineCourses.Models
{
    public class Comment
    {
        public int ID { get; set; }
        public ApplicationUser User {get; set;}
        public string Text{get;set;}
        public DateTime Date{get;set;}
        public CommentStatus Status { get; set; }=CommentStatus.Normal;
    }
}
