using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Galchenko.Data;
using Galchenko.Models;
using Galchenko.Models.Sports;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Galchenko.Services
{
    public class DbScaffoldingService
    {
        public static async Task Seed(IServiceProvider serviceProvider)
        {
            // ROLES
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            if (await roleManager.RoleExistsAsync("Moderator"))
                return;
                await roleManager.CreateAsync(new IdentityRole("Moderator"));
            if (!await roleManager.RoleExistsAsync("Student"))
                await roleManager.CreateAsync(new IdentityRole("Student"));
            if (!await roleManager.RoleExistsAsync("Referee"))
                await roleManager.CreateAsync(new IdentityRole("Referee"));
            if (!await roleManager.RoleExistsAsync("Coach"))
                await roleManager.CreateAsync(new IdentityRole("Coach"));

            // USERS
            ApplicationUser userModerator1 = new ApplicationUser { UserName = "moderator1", Email = "moderator1@gmail.com", FirstName = "Данила", LastName = "Крюков" };
            ApplicationUser userModerator2 = new ApplicationUser { UserName = "moderator2", Email = "moderator2@gmail.com", FirstName = "Кирилл", LastName = "Мамонтов" };
            ApplicationUser userStudent1 = new ApplicationUser { UserName = "student1", Email = "student1@gmail.com", FirstName = "Даниил", LastName = "Абрамов" };
            ApplicationUser userStudent2 = new ApplicationUser { UserName = "student2", Email = "student2@gmail.com", FirstName = "Григорий", LastName = "Казаков" };
            ApplicationUser userStudent3 = new ApplicationUser { UserName = "student3", Email = "student3@gmail.com", FirstName = "Вадим", LastName = "Ильин" };
            ApplicationUser userStudent4 = new ApplicationUser { UserName = "student4", Email = "student4@gmail.com", FirstName = "Ярослав", LastName = "Фомин" };
            ApplicationUser userReferee1 = new ApplicationUser { UserName = "referee1", Email = "referee1@gmail.com", FirstName = "Алексей", LastName = "Блохин" };
            ApplicationUser userReferee2 = new ApplicationUser { UserName = "referee2", Email = "referee2@gmail.com", FirstName = "Богдан", LastName = "Одинцов" };
            ApplicationUser userCoach1 = new ApplicationUser { UserName = "coach1", Email = "coach1@gmail.com", FirstName = "Евгений", LastName = "Моисеев" };
            ApplicationUser userCoach2 = new ApplicationUser { UserName = "coach2", Email = "coach2@gmail.com", FirstName = "Денис", LastName = "Киселёв" };

            {
                var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                if (await userManager.FindByEmailAsync("moderator1@gmail.com") == null)
                {
                    await userManager.CreateAsync(userModerator1, "moderator1");
                    await userManager.AddToRoleAsync(userModerator1, "Moderator");
                }
                if (await userManager.FindByEmailAsync("moderator2@gmail.com") == null)
                {
                    await userManager.CreateAsync(userModerator2, "moderator2");
                    await userManager.AddToRoleAsync(userModerator2, "Moderator");
                }
                if (await userManager.FindByEmailAsync("student1@gmail.com") == null)
                {
                    await userManager.CreateAsync(userStudent1, "student1");
                    await userManager.AddToRoleAsync(userStudent1, "Student");
                }
                if (await userManager.FindByEmailAsync("student2@gmail.com") == null)
                {
                    await userManager.CreateAsync(userStudent2, "student2");
                    await userManager.AddToRoleAsync(userStudent2, "Student");
                }
                if (await userManager.FindByEmailAsync("student3@gmail.com") == null)
                {
                    await userManager.CreateAsync(userStudent3, "userStudent3");
                    await userManager.AddToRoleAsync(userStudent3, "Student");
                }
                if (await userManager.FindByEmailAsync("student4@gmail.com") == null)
                {
                    await userManager.CreateAsync(userStudent4, "student4");
                    await userManager.AddToRoleAsync(userStudent4, "Student");
                }
                if (await userManager.FindByEmailAsync("referee1@gmail.com") == null)
                {
                    await userManager.CreateAsync(userReferee1, "referee1");
                    await userManager.AddToRoleAsync(userReferee1, "Referee");
                }
                if (await userManager.FindByEmailAsync("referee2@gmail.com") == null)
                {
                    await userManager.CreateAsync(userReferee2, "referee2");
                    await userManager.AddToRoleAsync(userReferee2, "Referee");
                }
                if (await userManager.FindByEmailAsync("coach1@gmail.com") == null)
                {
                    await userManager.CreateAsync(userCoach1, "coach1");
                    await userManager.AddToRoleAsync(userCoach1, "Coach");
                }
                if (await userManager.FindByEmailAsync("coach2@gmail.com") == null)
                {
                    await userManager.CreateAsync(userCoach2, "coach2");
                    await userManager.AddToRoleAsync(userCoach2, "Coach");
                }
            }

            using (var context = new ApplicationDbContext())
            {
                KindOfSport sport1, sport2;
                {
                    sport1 = new KindOfSport
                    {
                        Sport = "Бейсбол"
                    };
                    sport2 = new KindOfSport
                    {
                        Sport = "Футбол"
                    };

                    context.KindOfSports.Add(sport1);
                    context.KindOfSports.Add(sport2);
                }
                
                Student stud1, stud2, stud3, stud4;
                {
                    stud1 = new Student
                    {
                        ApplicationUserId = userStudent1.Id
                    };
                    stud2 = new Student
                    {
                        ApplicationUserId = userStudent2.Id
                    };
                    stud3 = new Student
                    {
                        ApplicationUserId = userStudent3.Id
                    };
                    stud4 = new Student
                    {
                        ApplicationUserId = userStudent4.Id
                    };
                    context.Students.Add(stud1);
                    context.Students.Add(stud2);
                    context.Students.Add(stud3);
                    context.Students.Add(stud4);
                }

                Referee ref1, ref2;
                {
                    ref1 = new Referee
                    {
                        ApplicationUserId = userReferee1.Id
                    };
                    ref2 = new Referee
                    {
                        ApplicationUserId = userReferee2.Id
                    };
                    context.Referees.Add(ref1);
                    context.Referees.Add(ref2);
                }

                Moderator mod1, mod2;
                {
                    mod1 = new Moderator
                    {
                        ApplicationUserId = userModerator1.Id
                    };
                    mod2 = new Moderator
                    {
                        ApplicationUserId = userModerator2.Id
                    };
                    context.Moderators.Add(mod1);
                    context.Moderators.Add(mod2);
                }

                Coach coach1 = new Coach
                {
                    ApplicationUserId = userCoach1.Id
                };
                Coach coach2 = new Coach
                {
                    ApplicationUserId = userCoach2.Id
                };

                context.Coaches.Add(coach1);
                context.Coaches.Add(coach2);
                context.SaveChanges();

                Competition comp1, comp2, comp3, comp4;
                {
                    comp1 = new Competition
                    {
                        KindOfSportId = sport1.Id,
                        Name = "Змагання 1",
                        Place = "Місце проведення 1",
                        RefereeId = ref1.ApplicationUserId,
                        Date = DateTime.Today,
                        ModeratorId = mod1.ApplicationUserId
                    };
                    comp4 = new Competition
                    {
                        KindOfSportId = sport1.Id,
                        Name = "Змагання 4",
                        Place = "Місце проведення 4",
                        RefereeId = ref1.ApplicationUserId,
                        Date = DateTime.Today.AddDays(18),
                        ModeratorId = mod1.ApplicationUserId
                    };
                    comp2 = new Competition
                    {
                        KindOfSportId = sport2.Id,
                        Name = "Змагання 2",
                        Place = "Місце проведення 2",
                        RefereeId = ref2.ApplicationUserId,
                        Date = DateTime.Today.AddDays(5),
                        ModeratorId = mod2.ApplicationUserId
                    };
                    comp3 = new Competition
                    {
                        KindOfSportId = sport2.Id,
                        Name = "Змагання 3",
                        Place = "Місце проведення 3",
                        RefereeId = ref2.ApplicationUserId,
                        Date = DateTime.Today.AddDays(40),
                        ModeratorId = mod2.ApplicationUserId
                    };
                    context.Competitions.Add(comp1);
                    context.Competitions.Add(comp2);
                    context.Competitions.Add(comp3);
                    context.Competitions.Add(comp4);
                }
                
                context.SaveChanges();
                                
                Team team1 = new Team
                {
                    KindOfSportId = sport1.Id,
                    Name = "Tigers",
                    CoachId = coach1.ApplicationUserId,
                    Students = new List<Student> { stud1 }
                };
                Team team2 = new Team
                {
                    KindOfSportId = sport2.Id,
                    Name = "Sharks",
                    CoachId = coach2.ApplicationUserId,
                    Students = new List<Student> { stud2 }
                };
                context.Teams.Add(team1);
                context.Teams.Add(team2);
                context.SaveChanges();
                
                coach1.Team = team1;
                coach2.Team = team2;
                
                context.SaveChanges();
                
                context.TeamJoinRequests.Add(new TeamJoinRequest
                {
                    Team = team1,
                    Student = stud1,
                    Result = false
                });
                context.TeamJoinRequests.Add(new TeamJoinRequest
                {
                    Team = team1,
                    Student = stud1,
                    Result = true
                });
                context.TeamJoinRequests.Add(new TeamJoinRequest
                {
                    Team = team2,
                    Student = stud2,
                    Result = true
                });
                
                stud1.Team = team1;
                stud2.Team = team2;
                
                context.SaveChanges();
                
                var cjr1 = new CompetitionJoinRequest
                {
                    Competition = comp1,
                    Team = team1,
                };
                var cjr2 = new CompetitionJoinRequest
                {
                    Competition = comp1,
                    Team = team2,
                };
                var cjr3 = new CompetitionJoinRequest
                {
                    Competition = comp2,
                    Team = team1,
                };
                var cjr4 = new CompetitionJoinRequest
                {
                    Competition = comp2,
                    Team = team2,
                };
                
                context.CompetitionJoinRequests.Add(cjr1);
                context.CompetitionJoinRequests.Add(cjr2);
                context.CompetitionJoinRequests.Add(cjr3);
                context.CompetitionJoinRequests.Add(cjr4);
                context.SaveChanges();
            }
        }
    }
}
