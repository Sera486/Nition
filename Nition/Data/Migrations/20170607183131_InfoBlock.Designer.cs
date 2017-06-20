using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Nition.Data.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20170607183131_InfoBlock")]
    partial class InfoBlock
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

            modelBuilder.Entity("Nition.Models.ApplicationUser", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

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

            modelBuilder.Entity("Nition.Models.Comment", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("Date");

                    b.Property<int?>("LessonID");

                    b.Property<string>("Text");

                    b.Property<string>("UserId");

                    b.HasKey("ID");

                    b.HasIndex("LessonID");

                    b.HasIndex("UserId");

                    b.ToTable("Comments");
                });

            modelBuilder.Entity("Nition.Models.Course", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AuthorId");

                    b.Property<DateTime>("CreationDate");

                    b.Property<string>("Description");

                    b.Property<double>("Estimate");

                    b.Property<string>("ImageURL");

                    b.Property<DateTime?>("ModificationDate");

                    b.Property<double>("Price");

                    b.Property<int>("PublishStatus");

                    b.Property<string>("Title");

                    b.HasKey("ID");

                    b.HasIndex("AuthorId");

                    b.ToTable("Courses");
                });

            modelBuilder.Entity("Nition.Models.CourseTheme", b =>
                {
                    b.Property<int>("CourseID");

                    b.Property<int>("ThemeID");

                    b.HasKey("CourseID", "ThemeID");

                    b.HasIndex("ThemeID");

                    b.ToTable("CourseThemes");
                });

            modelBuilder.Entity("Nition.Models.Lesson", b =>
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

            modelBuilder.Entity("Nition.Models.ManageViewModels.FamilyMember", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("MemberId");

                    b.Property<string>("UserId");

                    b.HasKey("ID");

                    b.HasIndex("MemberId");

                    b.HasIndex("UserId");

                    b.ToTable("FamilyMembers");
                });

            modelBuilder.Entity("Nition.Models.Subscription", b =>
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

            modelBuilder.Entity("Nition.Models.TextBlock", b =>
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

            modelBuilder.Entity("Nition.Models.Theme", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.HasKey("ID");

                    b.ToTable("Themes");
                });

            modelBuilder.Entity("Nition.Models.VideoBlock", b =>
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
                    b.HasOne("Nition.Models.ApplicationUser")
                        .WithMany("Claims")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("Nition.Models.ApplicationUser")
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

                    b.HasOne("Nition.Models.ApplicationUser")
                        .WithMany("Roles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Nition.Models.Comment", b =>
                {
                    b.HasOne("Nition.Models.Lesson", "Lesson")
                        .WithMany("Comments")
                        .HasForeignKey("LessonID");

                    b.HasOne("Nition.Models.ApplicationUser", "User")
                        .WithMany("Comments")
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("Nition.Models.Course", b =>
                {
                    b.HasOne("Nition.Models.ApplicationUser", "Author")
                        .WithMany("CreatedCourses")
                        .HasForeignKey("AuthorId");
                });

            modelBuilder.Entity("Nition.Models.CourseTheme", b =>
                {
                    b.HasOne("Nition.Models.Course", "Course")
                        .WithMany("CourseThemes")
                        .HasForeignKey("CourseID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Nition.Models.Theme", "Theme")
                        .WithMany("CourseThemes")
                        .HasForeignKey("ThemeID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Nition.Models.Lesson", b =>
                {
                    b.HasOne("Nition.Models.Course", "Course")
                        .WithMany("Lessons")
                        .HasForeignKey("CourseID");
                });

            modelBuilder.Entity("Nition.Models.ManageViewModels.FamilyMember", b =>
                {
                    b.HasOne("Nition.Models.ApplicationUser", "Member")
                        .WithMany("FamilyMembers")
                        .HasForeignKey("MemberId");

                    b.HasOne("Nition.Models.ApplicationUser", "User")
                        .WithMany()
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("Nition.Models.Subscription", b =>
                {
                    b.HasOne("Nition.Models.Course", "Course")
                        .WithMany("Subscriptions")
                        .HasForeignKey("CourseID");

                    b.HasOne("Nition.Models.ApplicationUser", "User")
                        .WithMany("Subscriptions")
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("Nition.Models.TextBlock", b =>
                {
                    b.HasOne("Nition.Models.Lesson", "Lesson")
                        .WithMany("TextBlocks")
                        .HasForeignKey("LessonID");
                });

            modelBuilder.Entity("Nition.Models.VideoBlock", b =>
                {
                    b.HasOne("Nition.Models.Lesson", "Lesson")
                        .WithMany("VideoBlocks")
                        .HasForeignKey("LessonID");
                });
        }
    }
}
