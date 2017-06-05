using System.Collections.Generic;
using System.Linq;
using Galchenko.Data;
using Galchenko.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Galchenko.Controllers
{
    [Route("api/coaches")]
    public class CoachController : Controller
    {
        private readonly ApplicationDbContext _dbContext = new ApplicationDbContext();

        [HttpGet("all")]
        public IEnumerable<CoachViewModel> All()
        {

            var coaches = _dbContext.Coaches
                .Include(coach => coach.ApplicationUser)
                .Include(coach => coach.Team)
                .ToArray();

            var result = coaches.Select(coach =>
            {
                if (coach.Team != null)
                {
                    return new CoachViewModel
                    {
                        Id = coach.ApplicationUserId,
                        UserName = coach.ApplicationUser.Email,
                        FirstName = coach.ApplicationUser.FirstName,
                        LastName = coach.ApplicationUser.LastName,
                        Team = new TeamViewModel
                        {
                            Id = coach.Team.Id,
                            Name = coach.Team.Name
                        }
                    };
                }
                {
                    return new CoachViewModel
                    {
                        Id = coach.ApplicationUserId,
                        UserName = coach.ApplicationUser.Email,
                        FirstName = coach.ApplicationUser.FirstName,
                        LastName = coach.ApplicationUser.LastName
                    };
                }
            }).ToArray();

            return result;
        }

        [HttpGet("{id}")]
        public CoachViewModel ByCoachId([FromRoute] string id)
        {
            var result = _dbContext.Coaches.Where(coach => coach.ApplicationUserId == id)
                .Include(coach => coach.ApplicationUser)
                .Select(coach => new CoachViewModel
                {
                    Id = coach.ApplicationUserId,
                    UserName = coach.ApplicationUser.UserName,
                    LastName = coach.ApplicationUser.LastName,
                    FirstName = coach.ApplicationUser.FirstName,
                }).FirstOrDefault();

            return result;
        }
    }
}
