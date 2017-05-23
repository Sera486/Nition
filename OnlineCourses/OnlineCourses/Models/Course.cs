using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineCourses.Models
{
    public class Course
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public ApplicationUser Creator { get; set; }
        public List<Subscription> Subscriptions { get; set; }
        public List<Lesson> Lessons { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime ModificationDate {get;set; }
        public string Description { get; set; }
        public string ImageURL { get; set; }
    }
}