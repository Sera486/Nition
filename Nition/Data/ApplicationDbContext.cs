using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Nition.Models;

namespace Nition.Data
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
        public DbSet<InfoBlock> InfoBlocks { get; set; }
        public DbSet<TextBlock> TextBlocks {get; set;}
        public DbSet<VideoBlock> VideoBlocks {get; set;}
        public DbSet<Subscription> Subscriptions{ get;set;}
        public DbSet<FamilyMember> FamilyMembers { get; set; }
        public DbSet<Theme> Themes { get; set; }
        public DbSet<CourseTheme> CourseThemes { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Course>(entity =>
            {
                entity.HasMany(e => e.Lessons).WithOne(e=>e.Course);
                entity.HasOne(e => e.Author).WithMany(e => e.CreatedCourses);
                entity.HasMany(c => c.Comments).WithOne();
                entity.Property(c=>c.CreationDate).HasColumnType("Date").HasDefaultValueSql("GetDate()");
                entity.Property(c => c.ModificationDate).HasColumnType("Date").HasDefaultValueSql("GetDate()");
            });

            builder.Entity<CourseTheme>(entity =>
            {
                entity.HasKey(e => new { e.CourseID, e.ThemeID });
                entity.HasOne(e => e.Course).WithMany(e => e.CourseThemes).HasForeignKey(e => e.CourseID);
                entity.HasOne(e => e.Theme).WithMany(e => e.CourseThemes).HasForeignKey(e => e.ThemeID);
            });
            
            builder.Entity<Lesson>(entity =>
            {
                entity.HasMany(e => e.TextBlocks).WithOne(e=>e.Lesson);
                entity.HasMany(e => e.VideoBlocks).WithOne(e=>e.Lesson);
                entity.HasMany(c => c.Comments).WithOne();
            });

            builder.Entity<Comment>(entity =>
            {
                entity.HasOne(e => e.User).WithMany(e => e.Comments);
                entity.Property(c => c.Date).HasColumnType("Date").HasDefaultValueSql("GetDate()");
            });

            builder.Entity<Subscription>(entity =>
            {
                entity.HasOne(e => e.User).WithMany(e => e.Subscriptions);
                entity.HasOne(e => e.Course).WithMany(e => e.Subscriptions);
            });

            builder.Entity<FamilyMember>(entity =>
            {
                entity.HasKey(e => new { e.UserID, e.MemberID });
                entity.HasOne(e => e.User).WithMany(e => e.FamilyMembers).HasForeignKey(e => e.UserID).OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(e => e.Member).WithMany(e=>e.SharingUsers).HasForeignKey(e => e.MemberID).OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<InfoBlock>(entity =>
            {
                entity.Ignore(e => e.Lesson);
            });
        }

        public DbSet<ApplicationUser> ApplicationUser { get; set; }
    }
}
