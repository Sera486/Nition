using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OnlineCourses.Models;
using OnlineCourses.Models.ManageViewModels;

namespace OnlineCourses.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Course> Courses {get; set;}
        public DbSet<Lesson> Lessons { get; set; }
        public DbSet<Comment> Comments {get; set;}
        public DbSet<TextBlock> TextBlocks {get; set;}
        public DbSet<VideoBlock> VideoBlocks {get; set;}
        public DbSet<Subscription> Subscriptions{ get;set;}
        public DbSet<FamilyMember> FamilyMembers { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Course>(entity =>
            {
                entity.HasMany(e => e.Lessons).WithOne(e=>e.Course);
                entity.HasOne(e => e.Author).WithMany(e => e.CreatedCourses);
            });

            builder.Entity<Lesson>(entity =>
            {
                entity.HasMany(e => e.TextBlocks).WithOne(e=>e.Lesson);
                entity.HasMany(e => e.VideoBlocks).WithOne(e=>e.Lesson);
            });

            builder.Entity<Comment>(entity =>
            {
                entity.HasOne(e => e.User).WithMany(e => e.Comments);
                entity.HasOne(e => e.Lesson).WithMany(e => e.Comments);
            });

            builder.Entity<Subscription>(entity =>
            {
                entity.HasOne(e => e.User).WithMany(e => e.Subscriptions);
                entity.HasOne(e => e.Course).WithMany(e => e.Subscriptions);
            });

            builder.Entity<FamilyMember>(entity =>
            {
                entity.ToTable("FamilyMembers");
                entity.HasOne(e => e.User).WithMany();
                entity.HasOne(e => e.Member).WithMany(e => e.FamilyMembers);
            });
        }

        public DbSet<OnlineCourses.Models.ApplicationUser> ApplicationUser { get; set; }
    }
}
