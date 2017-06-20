using System;

namespace Nition.Models
{
    public class Subscription
    {
        public int ID { get; set; }
        public ApplicationUser User { get; set; }
        public Course Course { get; set; }
        public DateTime SubscriptionDate { get; set; }
    }
}
