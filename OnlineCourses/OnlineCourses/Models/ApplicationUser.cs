using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace OnlineCourses.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        public string FirstName {get; set;}
        public string LastName {get; set;}
        public string ImageURL{get; set;}
        public string AboutMe { get; set; }
        public string Contacts { get; set; }
        public List<Subscription> Subscriptions { get; set; }
        public List<Course> CreatedCourses { get; set; }
        public List<Comment> Comments { get; set; }
        public List<FamilyMember> FamilyMembers { get; set; }
        /*
        public List<Subscription> ValidSubscriptions=> FamilyMembers == null
                    ? Subscriptions
                    : Subscriptions
                        .Concat(FamilyMembers.SelectMany(m => m.Member.Subscriptions)).Distinct().ToList();
         */

        
        public string FullName => $"{LastName} {FirstName}";
        public string ValidImageURL => string.IsNullOrWhiteSpace(ImageURL) ? "img/no_image.png" : ImageURL;
    }
}
