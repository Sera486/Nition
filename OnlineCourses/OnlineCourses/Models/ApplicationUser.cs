using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using OnlineCourses.Models.ManageViewModels;

namespace OnlineCourses.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        public string FirstName {get; set;}
        public string LastName {get; set;}
        public string ImageURL{get; set;}
        public List<Subscription> Subscriptions { get; set; }
        public List<Course> CreatedCourses { get; set; }
        public List<Comment> Comments { get; set; }
        public List<FamilyMember> FamilyMembers { get; set; }
    }
}
