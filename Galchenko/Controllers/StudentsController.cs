using System.Collections.Generic;
using System.Linq;
using Galchenko.Data;
using Galchenko.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Galchenko.Controllers
{
    [Route("api/users/students")]
    public class StudentsController : Controller
    {
        private readonly ApplicationDbContext _dbContext = new ApplicationDbContext();
        
        [HttpGet("[action]")]
        public IEnumerable<StudentViewModel> All()
        {
            var students = _dbContext.Students
                .Include(student => student.ApplicationUser)
                .Include(student => student.Team)
                .ToArray();

            var result = students.Select(student =>
            {
                if (student.Team != null)
                {
                    return new StudentViewModel
                    {
                        Id = student.ApplicationUserId,
                        FirstName = student.ApplicationUser.FirstName,
                        LastName = student.ApplicationUser.LastName,
                        UserName = student.ApplicationUser.Email,
                        Team = new TeamViewModel
                        {
                            Id = student.Team.Id,
                            KindOfSport = student.Team.KindOfSport,
                            Name = student.Team.Name,
                        }
                    };
                }
                return new StudentViewModel
                {
                    Id = student.ApplicationUserId,
                    FirstName = student.ApplicationUser.FirstName,
                    LastName = student.ApplicationUser.LastName,
                    UserName = student.ApplicationUser.Email,
//                    Team = new TeamViewModel
//                    {
//                    }
                };
            });

            return result;
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Student")]
        public StudentViewModel StudentFullProfile([FromRoute] string id)
        {
            var result = _dbContext.Students
                .Where(student => student.ApplicationUserId == id)
                .Include(student => student.ApplicationUser)
                .Include(student => student.Team)
                .Include(student => student.Team.Coach)
                .Include(student => student.Team.KindOfSport)
                .FirstOrDefault();

            if (result.Team != null)
            {
                return new StudentViewModel
                {
                    Id = result.ApplicationUserId,
                    FirstName = result.ApplicationUser.FirstName,
                    LastName = result.ApplicationUser.LastName,
                    UserName = result.ApplicationUser.Email,
                    Team = new TeamViewModel
                    {
                        Id = result.Team.Id,
                        KindOfSport = result.Team.KindOfSport,
                        Name = result.Team.Name,
//                        Coach = new CoachViewModel
//                        {
//                            Id = result.Team.Coach.ApplicationUserId,
//                            UserName = result.Team.Coach.ApplicationUser.UserName,
//                            FirstName = result.Team.Coach.ApplicationUser.FirstName,
//                            LastName = result.Team.Coach.ApplicationUser.LastName
//                        },
                        Students = result.Team.Students.Select(student => new StudentViewModel
                        {
                            Id = student.ApplicationUserId,
                            UserName = student.ApplicationUser.UserName,
                            FirstName = student.ApplicationUser.FirstName,
                            LastName = student.ApplicationUser.LastName,
                        }).ToArray()
                    }
                };
            }
            return new StudentViewModel
            {
                Id = result.ApplicationUserId,
                FirstName = result.ApplicationUser.FirstName,
                LastName = result.ApplicationUser.LastName,
                UserName = result.ApplicationUser.Email
            };
        }
    }
}
