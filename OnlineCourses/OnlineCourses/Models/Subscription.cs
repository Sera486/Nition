using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineCourses.Models
{
    public class Subscription
    {
        public int ID { get; set; }
        public ApplicationUser User { get; set; }
        public Course Course { get; set; }
        public DateTime SubscriptionDate { get; set; }
    }
}
