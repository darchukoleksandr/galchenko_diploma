using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Galchenko.Data;
using Galchenko.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Galchenko.Controllers
{
    [Route("api/teams")]
    public class TeamController : Controller
    {
        private readonly ApplicationDbContext _dbContext = new ApplicationDbContext();

        [HttpGet("all")]
        public IEnumerable<TeamViewModel> All()
        {
            var result = _dbContext.Teams
                .Include(team => team.Coach)
                .Include(team => team.KindOfSport)
                .Include(team => team.Students)
                .Select(team =>
                
                    new TeamViewModel
                    {
                        Coach = new CoachViewModel
                        {
                            Id = team.Coach.ApplicationUserId,
                            UserName = team.Coach.ApplicationUser.UserName,
                            LastName = team.Coach.ApplicationUser.LastName,
                            FirstName = team.Coach.ApplicationUser.FirstName
                        },
                        Name = team.Name,
                        KindOfSport = team.KindOfSport,
                        Id = team.Id,
                        Students = team.Students.Select(student => new StudentViewModel
                        {
                            Id = student.ApplicationUserId,
                            UserName = student.ApplicationUser.UserName,
                            LastName = student.ApplicationUser.LastName,
                            FirstName = student.ApplicationUser.FirstName

                        }).ToArray(),
                        Rating = (int)
                        (_dbContext.CompetitionPoints
                             .Where(points => points.Student.TeamId == team.Id)
                             .Select(points => (double)points.Score).Sum(score => score) /
                         _dbContext.CompetitionPoints
                             //                        .Where(points => points.Student.TeamId == team.Id)
                             .Select(points => (double)points.Score).Sum(score => score) * 100)
                        //                    Rating = new Random().Next(20, 45)
                    }
                )
                .ToArray();

            return result;
        }

        [HttpGet("coach/{id}")]
        public IActionResult ByCoachId([FromRoute] string id)
        {
            var result = _dbContext.Teams
                .Where(team => team.CoachId == id)
                //                .Include(team => team.Coach)
                .Include(team => team.KindOfSport)
                .Include(team => team.Students)
                .Select(team => new TeamViewModel
                {
                    Coach = new CoachViewModel
                    {
                        Id = team.Coach.ApplicationUserId,
//                        KindOfSport = team.Coach.KindOfSport,
                        UserName = team.Coach.ApplicationUser.UserName,
                        LastName = team.Coach.ApplicationUser.LastName,
                        FirstName = team.Coach.ApplicationUser.FirstName
                    },
                    Name = team.Name,
                    KindOfSport = team.KindOfSport,
                    Id = team.Id,
                    Students = team.Students.Select(student => new StudentViewModel
                    {
                        Id = student.ApplicationUserId,
                        UserName = student.ApplicationUser.UserName,
                        LastName = student.ApplicationUser.LastName,
                        FirstName = student.ApplicationUser.FirstName
                    }).ToArray()
                }).FirstOrDefault();
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }
        [HttpGet("{id}")]
        public IActionResult ById([FromRoute] int id)
        {
            var result = _dbContext.Teams
                .Where(team => team.Id == id)
                .Include(team => team.Coach)
                .Include(team => team.KindOfSport)
                .Include(team => team.Students)
                .Select(team => new TeamViewModel
                {
                    Coach = new CoachViewModel
                    {
                        Id = team.Coach.ApplicationUserId,
//                        KindOfSport = team.Coach.KindOfSport,
                        UserName = team.Coach.ApplicationUser.UserName,
                        LastName = team.Coach.ApplicationUser.LastName,
                        FirstName = team.Coach.ApplicationUser.FirstName
                    },
                    Name = team.Name,
                    KindOfSport = team.KindOfSport,
                    Id = team.Id,
                    Students = team.Students.Select( student => new StudentViewModel
                    {
                        Id = student.ApplicationUserId,
                        UserName = student.ApplicationUser.UserName,
                        LastName = student.ApplicationUser.LastName,
                        FirstName = student.ApplicationUser.FirstName,
//                        Rating = _dbContext.CompetitionPoints.Where(points =>
//                            points.Student.ApplicationUserId == student.ApplicationUserId)
//                            .Select(points => (double) points.Score)
//                            .Sum(score => score) / 
//                            _dbContext.CompetitionPoints
//                            .Select(points => (double)points.Score)
//                            .Sum(score => score) * 100
                    }).ToArray()
                }).FirstOrDefault();
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        [HttpPost("create")]
        [Authorize(Roles = "Coach")]
        public async Task<IActionResult> Create([FromBody] CreateTeamViewModel team)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var coach = _dbContext.Coaches.Find(team.CoachId);
            if (coach == null)
            {
                return NotFound("No such coach found in database!");
            }

            var result = new Team
            {
                CoachId = team.CoachId,
                KindOfSportId = team.KindOfSportId,
                Name = team.Name
            };

            coach.Team = result;
            await _dbContext.Teams.AddAsync(result);
            await _dbContext.SaveChangesAsync();

            return Ok();
        }

        [HttpPost("update")]
        [Authorize(Roles = "Coach")]
        public async Task<IActionResult> Update([FromBody] TeamViewModel team)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var result = _dbContext.Teams.Find(team.Id);
            if (result == null)
            {
                return NotFound("Team not found");
            }

            result.Name = team.Name;
            result.KindOfSportId = team.KindOfSport.Id;

            await _dbContext.SaveChangesAsync();

            return Ok();
        }

        [HttpPost("expel")]
        [Authorize(Roles = "Coach")]
        public async Task<IActionResult> ExpelPlayerFromTeam([FromBody] StudentViewModel student)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var result = _dbContext.Students.Find(student.Id);
            if (result == null)
            {
                return NotFound("Student not found");
            }
            if (result.TeamId == null)
            {
                return BadRequest("Student dont have a team!");
            }

            result.TeamId = null;

            await _dbContext.SaveChangesAsync();

            return Ok();
        }
    }
}
