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
    [Route("api/teams/request")]
    public class TeamJoinRequestController : Controller
    {
        private readonly ApplicationDbContext _dbContext = new ApplicationDbContext();
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
//
        public TeamJoinRequestController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager
            ){
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpPost("join")]
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> SendJoinRequest([FromBody] TeamJoinRequestViewModel request)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var team = await _dbContext.Teams.FindAsync(request.Team.Id);
            var student = await _dbContext.Students.FindAsync(request.Student.Id);

            if (student == null)
            {
                return NotFound(request.Student);
            }
            if (team == null)
            {
                return NotFound(request.Team);
            }

            if (_dbContext.TeamJoinRequests
                .Any(joinRequest => joinRequest.Student.ApplicationUserId == request.Student.Id &&
                                    joinRequest.Team.Id == request.Team.Id && 
                                    joinRequest.Result == null))
                return StatusCode(203);

            await _dbContext.TeamJoinRequests.AddAsync(new TeamJoinRequest
            {
                Team = team,
                Student = student
            });

            await _dbContext.SaveChangesAsync();

            return Ok();
        }

        [HttpPost("leave")]
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> SendLeaveRequest()
        {
            if (User.Identity.IsAuthenticated)
            {
                var user = await _userManager.FindByNameAsync(User.Identity.Name);

                var student = _dbContext.Students.Find(user.Id);

                student.TeamId = null;
                student.Team = null;

                await _dbContext.SaveChangesAsync();

                return Ok();
            }

            return Unauthorized();
        }

        [HttpGet("all")]
        [Authorize(Roles = "Coach")]
        public async Task<IActionResult> ByCoachId()
        {
            if (User.Identity.IsAuthenticated)
            {
                var coach = await _userManager.FindByNameAsync(User.Identity.Name);

                var result = await _dbContext.TeamJoinRequests
                    .Include(request => request.Team)
                    .Where(request => 
                        request.Team.CoachId == coach.Id && 
                        request.Result == null)
                    .Select(request => new TeamJoinRequestViewModel
                    {
                        Id = request.Id,
                        Result = request.Result,
                        Student = new StudentViewModel
                        {
                            Id = request.Student.ApplicationUserId,
                            UserName = request.Student.ApplicationUser.UserName,
                            FirstName = request.Student.ApplicationUser.FirstName,
                            LastName = request.Student.ApplicationUser.LastName,
                            Team = new TeamViewModel
                            {
                                Name = request.Student.Team.Name
                            }
                        },
                        Team = new TeamViewModel
                        {
                            Id = request.Team.Id,
                            Name = request.Team.Name,
                        },
                    }).ToArrayAsync();

                return Ok(result);
            }

            return Unauthorized();
        }

        [HttpPost("addToTeam")]
        [Authorize(Roles = "Coach")]
        public async Task<IActionResult> AddToTeam([FromBody] TeamJoinRequestResultViewModel requestResult)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var coach = await _userManager.FindByNameAsync(User.Identity.Name);

            if ((await _dbContext.Teams.FindAsync(requestResult.TeamId)).CoachId != coach.Id)
                return NotFound("Coach not found!");

            (await _dbContext.TeamJoinRequests.FindAsync(requestResult.TeamJoinRequestId)).Result = requestResult.Result;

            if (requestResult.Result)
            {
                _dbContext.Students.Find(requestResult.StudentId).Team = _dbContext.Teams.Find(requestResult.TeamId);
            }

            await _dbContext.SaveChangesAsync();

            return Ok();
        }
    } 
}
