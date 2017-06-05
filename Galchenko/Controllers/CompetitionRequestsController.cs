using System.Linq;
using System.Threading.Tasks;
using Galchenko.Data;
using Galchenko.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Galchenko.Controllers
{
    [Route("api/competitions/requests")]
    public class CompetitionRequestsController : Controller
    {
        private readonly ApplicationDbContext _dbContext = new ApplicationDbContext();
        private readonly UserManager<ApplicationUser> _userManager;

        public CompetitionRequestsController(
            UserManager<ApplicationUser> userManager
        )
        {
            _userManager = userManager;
        }

        [HttpPost]
        [Authorize(Roles = "Coach")]
        public async Task<IActionResult> SendJoinRequest([FromBody] CompetitionJoinRequestViewModel request)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var team = await _dbContext.Teams.FindAsync(request.Team.Id);
            var competition = await _dbContext.Competitions.FindAsync(request.Competition.Id);

            if (competition == null)
            {
                return NotFound(request.Competition);
            }
            if (team == null)
            {
                return NotFound(request.Team);
            }

            if (_dbContext.CompetitionJoinRequests.Any(joinRequest =>
                joinRequest.Competition.Id == request.Competition.Id &&
                joinRequest.Team.Id == request.Team.Id)
            )
            {
                return StatusCode(203);
            }


            await _dbContext.CompetitionJoinRequests.AddAsync(new CompetitionJoinRequest
            {
                Team = team,
                Competition = competition
            });

            await _dbContext.SaveChangesAsync();

            return Ok();
        }

        [HttpPost("result")]
        [Authorize(Roles = "Moderator")]
        public async Task<IActionResult> ChangeRequestResult([FromBody] CompetitionJoinRequestResultViewModel requestResult)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            if (requestResult.Result == false)
            {
                if (_dbContext.CompetitionPoints.Any(points => points.Competition.Id == requestResult.CompetitionId))
                    return StatusCode(203);
            }

            var result = await _dbContext.CompetitionJoinRequests.FindAsync(requestResult.RequestId);
            result.Result = requestResult.Result;

            await _dbContext.SaveChangesAsync();

            return Ok();
        }

        [HttpGet("all")]
        [Authorize(Roles = "Moderator")]
        public async Task<IActionResult> CompetitionJoinRequests(int id)
        {
            var moderator = await _userManager.FindByNameAsync(User.Identity.Name);

            var result = await _dbContext.CompetitionJoinRequests
                .Include(request => request.Team)
                .Include(request => request.Competition)
                .Where(request => request.Competition.ModeratorId == moderator.Id)
                .Select(request => new CompetitionJoinRequestViewModel
                {
                    Id = request.Id,
                    Result = request.Result,
                    Team = new TeamViewModel
                    {
                        Id = request.Team.Id,
                        Name = request.Team.Name,
                        KindOfSport = request.Team.KindOfSport,
                        Coach = new CoachViewModel
                        {
                            Id = request.Team.Coach.ApplicationUserId,
                            FirstName = request.Team.Coach.ApplicationUser.FirstName,
                            LastName = request.Team.Coach.ApplicationUser.LastName
                        }
                    },
                    Competition = new CompetitionViewModel
                    {
                        Id = request.Competition.Id,
                        Name = request.Competition.Name
                    }
                }).ToArrayAsync();

            return Ok(result);
        }
    }
}
