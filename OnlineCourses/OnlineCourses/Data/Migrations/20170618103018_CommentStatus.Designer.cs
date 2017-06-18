using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using OnlineCourses.Data;
using OnlineCourses.Models.Enums;

namespace OnlineCourses.Data.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20170618103018_CommentStatus")]
    partial class CommentStatus
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.2")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("RoleId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("OnlineCourses.Models.ApplicationUser", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AboutMe");

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Contacts");

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<string>("FirstName");

                    b.Property<string>("ImageURL");

                    b.Property<string>("LastName");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("OnlineCourses.Models.Comment", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("CourseID");

                    b.Property<DateTime>("Date")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("Date")
                        .HasDefaultValueSql("GetDate()");

                    b.Property<int?>("LessonID");

                    b.Property<int>("Status");

                    b.Property<string>("Text");

                    b.Property<string>("UserId");

                    b.HasKey("ID");

                    b.HasIndex("CourseID");

                    b.HasIndex("LessonID");

                    b.HasIndex("UserId");

                    b.ToTable("Comments");
                });

            modelBuilder.Entity("OnlineCourses.Models.Course", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AuthorId");

                    b.Property<DateTime>("CreationDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("Date")
                        .HasDefaultValueSql("GetDate()");

                    b.Property<string>("Description");

                    b.Property<double>("Estimate");

                    b.Property<string>("ImageURL");

                    b.Property<DateTime?>("ModificationDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("Date")
                        .HasDefaultValueSql("GetDate()");

                    b.Property<double>("Price");

                    b.Property<int>("PublishStatus");

                    b.Property<string>("Title");

                    b.HasKey("ID");

                    b.HasIndex("AuthorId");

                    b.ToTable("Courses");
                });

            modelBuilder.Entity("OnlineCourses.Models.CourseTheme", b =>
                {
                    b.Property<int>("CourseID");

                    b.Property<int>("ThemeID");

                    b.HasKey("CourseID", "ThemeID");

                    b.HasIndex("ThemeID");

                    b.ToTable("CourseThemes");
                });

            modelBuilder.Entity("OnlineCourses.Models.FamilyMember", b =>
                {
                    b.Property<string>("UserID");

                    b.Property<string>("MemberID");

                    b.HasKey("UserID", "MemberID");

                    b.HasIndex("MemberID");

                    b.ToTable("FamilyMembers");
                });

            modelBuilder.Entity("OnlineCourses.Models.Lesson", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("CourseID");

                    b.Property<string>("Description");

                    b.Property<bool>("IsFree");

                    b.Property<int>("Order");

                    b.Property<string>("Title");

                    b.HasKey("ID");

                    b.HasIndex("CourseID");

                    b.ToTable("Lessons");
                });

            modelBuilder.Entity("OnlineCourses.Models.Subscription", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("CourseID");

                    b.Property<DateTime>("SubscriptionDate");

                    b.Property<string>("UserId");

                    b.HasKey("ID");

                    b.HasIndex("CourseID");

                    b.HasIndex("UserId");

                    b.ToTable("Subscriptions");
                });

            modelBuilder.Entity("OnlineCourses.Models.TextBlock", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("LessonID");

                    b.Property<int>("Order");

                    b.Property<string>("Text");

                    b.HasKey("ID");

                    b.HasIndex("LessonID");

                    b.ToTable("TextBlocks");
                });

            modelBuilder.Entity("OnlineCourses.Models.Theme", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.HasKey("ID");

                    b.ToTable("Themes");
                });

            modelBuilder.Entity("OnlineCourses.Models.VideoBlock", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("LessonID");

                    b.Property<int>("Order");

                    b.Property<string>("VideoURL");

                    b.HasKey("ID");

                    b.HasIndex("LessonID");

                    b.ToTable("VideoBlocks");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRole")
                        .WithMany("Claims")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("OnlineCourses.Models.ApplicationUser")
                        .WithMany("Claims")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("OnlineCourses.Models.ApplicationUser")
                        .WithMany("Logins")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRole")
                        .WithMany("Users")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("OnlineCourses.Models.ApplicationUser")
                        .WithMany("Roles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("OnlineCourses.Models.Comment", b =>
                {
                    b.HasOne("OnlineCourses.Models.Course")
                        .WithMany("Comments")
                        .HasForeignKey("CourseID");

                    b.HasOne("OnlineCourses.Models.Lesson")
                        .WithMany("Comments")
                        .HasForeignKey("LessonID");

                    b.HasOne("OnlineCourses.Models.ApplicationUser", "User")
                        .WithMany("Comments")
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("OnlineCourses.Models.Course", b =>
                {
                    b.HasOne("OnlineCourses.Models.ApplicationUser", "Author")
                        .WithMany("CreatedCourses")
                        .HasForeignKey("AuthorId");
                });

            modelBuilder.Entity("OnlineCourses.Models.CourseTheme", b =>
                {
                    b.HasOne("OnlineCourses.Models.Course", "Course")
                        .WithMany("CourseThemes")
                        .HasForeignKey("CourseID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("OnlineCourses.Models.Theme", "Theme")
                        .WithMany("CourseThemes")
                        .HasForeignKey("ThemeID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("OnlineCourses.Models.FamilyMember", b =>
                {
                    b.HasOne("OnlineCourses.Models.ApplicationUser", "Member")
                        .WithMany()
                        .HasForeignKey("MemberID");

                    b.HasOne("OnlineCourses.Models.ApplicationUser", "User")
                        .WithMany("FamilyMembers")
                        .HasForeignKey("UserID");
                });

            modelBuilder.Entity("OnlineCourses.Models.Lesson", b =>
                {
                    b.HasOne("OnlineCourses.Models.Course", "Course")
                        .WithMany("Lessons")
                        .HasForeignKey("CourseID");
                });

            modelBuilder.Entity("OnlineCourses.Models.Subscription", b =>
                {
                    b.HasOne("OnlineCourses.Models.Course", "Course")
                        .WithMany("Subscriptions")
                        .HasForeignKey("CourseID");

                    b.HasOne("OnlineCourses.Models.ApplicationUser", "User")
                        .WithMany("Subscriptions")
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("OnlineCourses.Models.TextBlock", b =>
                {
                    b.HasOne("OnlineCourses.Models.Lesson", "Lesson")
                        .WithMany("TextBlocks")
                        .HasForeignKey("LessonID");
                });

            modelBuilder.Entity("OnlineCourses.Models.VideoBlock", b =>
                {
                    b.HasOne("OnlineCourses.Models.Lesson", "Lesson")
                        .WithMany("VideoBlocks")
                        .HasForeignKey("LessonID");
                });
        }
    }
}
