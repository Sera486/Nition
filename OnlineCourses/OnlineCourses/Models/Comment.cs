using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineCourses.Models
{
    public class Comment
    {
        public int ID { get; set; }
        public ApplicationUser User {get; set;}
        public Lesson Lesson {get; set;}
        public string Text{get;set;}
        public DateTime Date{get;set;}
    }
}
