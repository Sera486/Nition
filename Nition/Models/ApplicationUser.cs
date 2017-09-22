using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace Nition.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName {get; set;}
        public string LastName {get; set;}
        public string ImageURL{get; set;}
        public string AboutMe { get; set; }
        public string Facebook { get; set; }
        public string Twitter { get; set; }
        public string Linkedin { get; set; }
        public string Skype { get; set; }
        public List<Subscription> Subscriptions { get; set; }
        public List<Course> CreatedCourses { get; set; }
        public List<Comment> Comments { get; set; }
        public List<FamilyMember> FamilyMembers { get; set; }
        public List<FamilyMember> SharingUsers { get; set; }

        public List<Subscription> ValidSubscriptions()
        {
            List<Subscription> rez = new List<Subscription>();
            if(Subscriptions!=null)rez.AddRange(Subscriptions);
            if(SharingUsers!=null)rez.AddRange(SharingUsers.SelectMany(m => m.User.Subscriptions));
            return rez.Distinct().ToList();
        }

        public string FullName => $"{LastName} {FirstName}";
        public string ValidImageURL => string.IsNullOrWhiteSpace(ImageURL) ? "img/no_image.png" : ImageURL;
    }


}
