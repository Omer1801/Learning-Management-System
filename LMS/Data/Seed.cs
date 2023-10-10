using LMS.Data;
using LMS.Models;
using Microsoft.AspNetCore.Identity;
using System.Diagnostics;
using System.Net;

namespace LMS.Data
{
    public class Seed
    {
        public static void SeedDataCourses(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();

                context.Database.EnsureCreated();

                if (!context.Courses.Any())
                {
                    context.Courses.AddRange(new List<Course>()
                    {
                        new Course()
                        {
                            Title = "Course 1",
                            InstructorId = context.Users.Where(u => u.Role=="instructor").ToList().First().Id,
                            Description = "This is the description of the course",
                            Category = "Course Category",
                            ImageURL = "https://t4.ftcdn.net/jpg/03/08/69/75/360_F_308697506_9dsBYHXm9FwuW0qcEqimAEXUvzTwfzwe.jpg",
                            EnrollmentCount = 0,
                        },
                        new Course()
                        {
                            Title = "Course 2",
                            InstructorId = context.Users.Where(u => u.Role=="instructor").ToList().First().Id,
                            Description = "This is the description of the course",
                            Category = "Course Category",
                            ImageURL = "https://t4.ftcdn.net/jpg/03/08/69/75/360_F_308697506_9dsBYHXm9FwuW0qcEqimAEXUvzTwfzwe.jpg",
                            EnrollmentCount = 0,
                        },
                        new Course()
                        {
                            Title = "Course 3",
                            InstructorId = context.Users.Where(u => u.Role=="instructor").ToList().First().Id,
                            Description = "This is the description of the course",
                            Category = "Course Category",
                            ImageURL = "https://st2.depositphotos.com/1907633/8862/i/450/depositphotos_88627992-stock-photo-business-man-hand-working-on.jpg",
                            EnrollmentCount = 0,
                        }
                    });
                    context.SaveChanges();
                }          
            }
        }
        public static void SeedDataRest(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();

                context.Database.EnsureCreated();

                if (!context.Assignments.Any())
                {
                    context.Assignments.AddRange(new List<Assignment>()
                    {
                        new Assignment()
                        {
                            Title = "Assignment 1",
                            Description = "This is the description of the assignment",
                            Content = "This is the content of the assignment",
                            DueDate = DateTime.Now,
                            CourseId = context.Courses.First().CourseId
                         },
                        new Assignment()
                        {
                            Title = "Assignment 2",
                            Description = "This is the description of the assignment",
                            Content = "This is the content of the assignment",
                            DueDate = DateTime.Now,
                            CourseId = context.Courses.First().CourseId
                         },
                        new Assignment()
                        {
                            Title = "Assignment 3",
                            Description = "This is the description of the assignment",
                            Content = "This is the content of the assignment",
                            DueDate = DateTime.Now,
                            CourseId = context.Courses.First().CourseId
                         },
                        new Assignment()
                        {
                            Title = "Assignment 4",
                            Description = "This is the description of the assignment",
                            Content = "This is the content of the assignment",
                            DueDate = DateTime.Now,
                            CourseId = context.Courses.First().CourseId
                         }
                    });
                    context.SaveChanges();
                }
                if (!context.Enrollments.Any())
                {
                    context.Enrollments.AddRange(new List<Enrollment>()
                    {
                        new Enrollment()
                        {
                            UserId = context.Users.Where(u => u.Role=="user").ToList().First().Id,
                            CourseId = context.Courses.First().CourseId

                        },
                        new Enrollment()
                        {
                            UserId = context.Users.Where(u => u.Role=="user").ToList().First().Id,
                            CourseId = context.Courses.OrderBy(j => j.CourseId).Skip(1).Take(1).FirstOrDefault().CourseId

                        }
                    });
                    context.SaveChanges();
                }
                if (!context.Lessons.Any())
                {
                    context.Lessons.AddRange(new List<Lesson>()
                    {
                        new Lesson()
                        {
                            Title = "Lesson 1",
                            Description = "This is the description of the Lesson",
                            Content = "This is the content of the Lesson",
                            CourseId = context.Courses.First().CourseId
                         },
                        new Lesson()
                        {
                            Title = "Lesson 2",
                            Description = "This is the description of the Lesson",
                            Content = "This is the content of the Lesson",
                            CourseId = context.Courses.First().CourseId
                         },
                        new Lesson()
                        {
                            Title = "Lesson 3",
                            Description = "This is the description of the Lesson",
                            Content = "This is the content of the Lesson",
                            CourseId = context.Courses.First().CourseId
                         }
                    });
                    context.SaveChanges();
                }
            }
        }

        public static async Task SeedUsersAndRolesAsync(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                //Roles
                var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                if (!await roleManager.RoleExistsAsync(UserRoles.Admin))
                    await roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));
                if (!await roleManager.RoleExistsAsync(UserRoles.User))
                    await roleManager.CreateAsync(new IdentityRole(UserRoles.User));
                if (!await roleManager.RoleExistsAsync(UserRoles.Instructor))
                    await roleManager.CreateAsync(new IdentityRole(UserRoles.Instructor));

                //Users
                var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<User>>();
                string adminUserEmail = "deneme@gmail.com";

                var adminUser = await userManager.FindByEmailAsync(adminUserEmail);
                if (adminUser == null)
                {
                    var newAdminUser = new User()
                    {
                        UserName = "admin",
                        Email = adminUserEmail,
                        EmailConfirmed = true,
                        Role = "admin",
                    };
                    await userManager.CreateAsync(newAdminUser, "Coding@1234?");
                    await userManager.AddToRoleAsync(newAdminUser, UserRoles.Admin);
                }

                string appUserEmail = "user@etickets.com";

                var appUser = await userManager.FindByEmailAsync(appUserEmail);
                if (appUser == null)
                {
                    var newAppUser = new User()
                    {
                        UserName = "app-user",
                        Email = appUserEmail,
                        EmailConfirmed = true,
                        Role = "user",
                    };
                    await userManager.CreateAsync(newAppUser, "Coding@1234?");
                    await userManager.AddToRoleAsync(newAppUser, UserRoles.User);
                }

                string appInstructorEmail = "instructor@etickets.com";

                var appInstructor = await userManager.FindByEmailAsync(appInstructorEmail);
                if (appInstructor == null)
                {
                    var newAppInstructor = new User()
                    {
                        UserName = "app-instructor",
                        Email = appInstructorEmail,
                        EmailConfirmed = true,
                        Role = "instructor",
                    };
                    await userManager.CreateAsync(newAppInstructor, "Coding@1234?");
                    await userManager.AddToRoleAsync(newAppInstructor, UserRoles.Instructor);
                }
            }
        }
    }
}