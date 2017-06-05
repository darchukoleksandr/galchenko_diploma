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
    [Route("api/competitions/points")]
    public class CompetitionsPointsController : Controller
    {
        private readonly ApplicationDbContext _dbContext = new ApplicationDbContext();

        [HttpPost("submit")]
        [Authorize(Roles = "Referee")]
        public async Task<IActionResult> PostCompetitionPoints([FromBody] CompetitionPointsViewModel point)
        {
            var competition = await _dbContext.Competitions.FindAsync(point.Competition.Id);
            if (competition == null)
                return NotFound();
            var student = await _dbContext.Students.FindAsync(point.Student.Id);
            if (student == null)
                return NotFound();

            var result = new CompetitionPoints
            {
                Score = point.Score,
                Competition = competition,
                Student = student,
            };

            await _dbContext.CompetitionPoints.AddAsync(result);
            await _dbContext.SaveChangesAsync();

            return Ok(result.Id);
        }

        [HttpPost("delete")]
        [Authorize(Roles = "Referee")]
        public async Task<IActionResult> DeleteCompetitionPoints([FromBody] CompetitionPointsViewModel point)
        {
            var result = await _dbContext.CompetitionPoints.FindAsync(point.Id);

            _dbContext.CompetitionPoints.Remove(result);

            await _dbContext.SaveChangesAsync();

            return Ok();
        }

        [HttpGet("results/{id}")]
        public async Task<IEnumerable<CompetitionPointsViewModel>> GetCompetitonResults([FromRoute] int id)
        {
            var result = await _dbContext.CompetitionPoints
                .Where(points => points.Competition.Id == id)
                .Select(points => new CompetitionPointsViewModel
                {
                    Id = points.Id,
                    Score = points.Score,
                    Student = new StudentViewModel
                    {
                        Id = points.Student.ApplicationUser.Id,
                        FirstName = points.Student.ApplicationUser.FirstName,
                        LastName = points.Student.ApplicationUser.LastName,
                        UserName = points.Student.ApplicationUser.UserName,
                        Team = new TeamViewModel
                        {
                            Id = points.Student.Team.Id,
                            Name = points.Student.Team.Name
                        }
                    }
                }).ToArrayAsync();

            return result;
        }

        [HttpGet("{id}")]
        public async Task<IEnumerable<CompetitionPointsViewModel>> GetCompetitionPoints([FromRoute] int id)
        {
            var result = await _dbContext.CompetitionPoints
                .Where(points => points.Competition.Id == id)
                .Select(points => new CompetitionPointsViewModel
                {
                    Id = points.Id,
                    Score = points.Score,
                    Student = new StudentViewModel
                    {
                        Id = points.Student.ApplicationUserId,
                        FirstName = points.Student.ApplicationUser.FirstName,
                        LastName = points.Student.ApplicationUser.LastName,
                        UserName = points.Student.ApplicationUser.UserName,
                        Team = new TeamViewModel
                        {
                            Id = points.Student.Team.Id,
                            Name = points.Student.Team.Name,
                        }
                    },
                    Competition = new CompetitionViewModel
                    {
                        Id = points.Competition.Id
                    }
                })
                .ToArrayAsync();

            return result;
        }

        [HttpGet("player/{id}")]
        public async Task<IEnumerable<CompetitionPointsViewModel>> GetCompetitionPointsByPlayerId([FromRoute] string id)
        {
            var result = await _dbContext.CompetitionPoints
                .Where(points => points.Student.ApplicationUserId == id)
                .Select(points => new CompetitionPointsViewModel
                {
                    Id = points.Id,
                    Score = points.Score,
//                    Student = new StudentViewModel
//                    {
//                        Id = points.Student.ApplicationUserId,
//                        FirstName = points.Student.ApplicationUser.FirstName,
//                        LastName = points.Student.ApplicationUser.LastName,
//                        UserName = points.Student.ApplicationUser.UserName,
//                        Team = new TeamViewModel
//                        {
//                            Id = points.Student.Team.Id,
//                            Name = points.Student.Team.Name,
//                        }
//                    },
                    Competition = new CompetitionViewModel
                    {
                        Id = points.Competition.Id,
                        Name = points.Competition.Name,
                        Date = points.Competition.Date,
                        Place = points.Competition.Place
                    }
                })
                .ToArrayAsync();

            return result;
        }

    }
}
