using System;
using System.Collections.Generic;
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
    [Route("api/competitions")]
    public class CompetitionsController : Controller
    {
        private readonly ApplicationDbContext _dbContext = new ApplicationDbContext();
        private readonly UserManager<ApplicationUser> _userManager;
//        private readonly SignInManager<ApplicationUser> _signInManager;
        
        public CompetitionsController(
            UserManager<ApplicationUser> userManager
//            SignInManager<ApplicationUser> signInManager,
        )
        {
            _userManager = userManager;
//            _signInManager = signInManager;
        }

        [HttpGet("all")]
        public IEnumerable<CompetitionViewModel> All()
        {
            return _dbContext.Competitions
                .Select(competition => new CompetitionViewModel
                {
                    Id = competition.Id,
                    Name = competition.Name,
                    Date = competition.Date,
                    Place = competition.Place,
                    KindOfSport = competition.KindOfSport,
                    Referee = new RefereeViewModel
                    {
                        Id = competition.Referee.ApplicationUserId,
                        FirstName = competition.Referee.ApplicationUser.FirstName,
                        LastName = competition.Referee.ApplicationUser.LastName,
                        UserName = competition.Referee.ApplicationUser.UserName,
//                        KindOfSport = competition.Referee.KindOfSport
                    },
                    Moderator = new ModeratorViewModel
                    {
                        Id = competition.Moderator.ApplicationUserId,
                        FirstName = competition.Moderator.ApplicationUser.FirstName,
                        LastName = competition.Moderator.ApplicationUser.LastName,
                        UserName = competition.Moderator.ApplicationUser.UserName
                    }
                })
                .ToArray();
        }

        [HttpGet("{id}")]
        public CompetitionViewModel ById(int id)
        {
            return _dbContext
                .Competitions
                .Where(competition => competition.Id == id)
                .Select(competition => new CompetitionViewModel
                {
                    Id = competition.Id,
                    Name = competition.Name,
                    Date = competition.Date,
                    Place = competition.Place,
                    KindOfSport = competition.KindOfSport,
                    Referee = new RefereeViewModel
                    {
                        Id = competition.Referee.ApplicationUserId,
                        FirstName = competition.Referee.ApplicationUser.FirstName,
                        LastName = competition.Referee.ApplicationUser.LastName,
                        UserName = competition.Referee.ApplicationUser.UserName,
//                        KindOfSport = competition.Referee.KindOfSport
                    }
                }).FirstOrDefault();
        }

        [HttpGet("moderator/{id}")]
        public CompetitionViewModel[] ByModeratorId(string id)
        {
            return _dbContext
                .Competitions
                .Where(competition => competition.ModeratorId == id)
                .Select(competition => new CompetitionViewModel
                {
                    Id = competition.Id,
                    Name = competition.Name,
                    Date = competition.Date,
                    Place = competition.Place,
                    KindOfSport = competition.KindOfSport,
                    Referee = new RefereeViewModel
                    {
                        Id = competition.Referee.ApplicationUserId,
                        FirstName = competition.Referee.ApplicationUser.FirstName,
                        LastName = competition.Referee.ApplicationUser.LastName,
                        UserName = competition.Referee.ApplicationUser.UserName,
//                        KindOfSport = competition.Referee.KindOfSport
                    }
                }).ToArray();
        }
        
        [HttpPost("create")]
        [Authorize(Roles = "Moderator")]
        public async Task<IActionResult> Create([FromBody] CreateCompetitionViewModel competition)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var result = new Competition
            {
                Date = competition.Date,
                KindOfSportId = competition.KindOfSportId,
                Name = competition.Name,
                Place = competition.Place,
                ModeratorId = competition.ModeratorId,
                RefereeId = competition.RefereeId
            };
            await _dbContext.Competitions.AddAsync(result);
            await _dbContext.SaveChangesAsync();
            return Ok();
        }

        [HttpGet("referee/{id}")]
        [Authorize(Roles = "Referee")]
        public async Task<IActionResult> ByRefereeId(string id)
        {
            var referee = await _userManager.FindByNameAsync(User.Identity.Name);

            var result = await _dbContext.Competitions
//                .Include(request => request.Competition)
                .Where(competition => competition.RefereeId == referee.Id)
                .Select(competition => new CompetitionViewModel
                {
                    Id = competition.Id,
                        Name = competition.Name,
                        Place = competition.Place,
                        Date = competition.Date,
                        Moderator = new ModeratorViewModel
                        {
                            Id = competition.Moderator.ApplicationUserId,
                            LastName = competition.Moderator.ApplicationUser.LastName,
                            FirstName = competition.Moderator.ApplicationUser.FirstName,
                            UserName = competition.Moderator.ApplicationUser.UserName
                        },
                        KindOfSport = competition.KindOfSport
                }).ToArrayAsync();

            return Ok(result);
        }

        [HttpGet("{id}/teams")]
        [Authorize(Roles = "Referee")]
        public async Task<IActionResult> TeamsByCompetitionId(int id)
        {
            var referee = await _userManager.FindByNameAsync(User.Identity.Name);

            var result = await _dbContext.CompetitionJoinRequests
                .Include(request => request.Team)
                .Include(request => request.Competition)
                .Where(request => 
                    request.Competition.RefereeId == referee.Id && 
                    request.Competition.Id == id && 
                    request.Result == true)
                .Select(request => new TeamViewModel
                {
                    Id = request.Team.Id,
                    Name = request.Team.Name,
                    KindOfSport = request.Team.KindOfSport,
                    Coach = new CoachViewModel
                    {
                        Id = request.Team.Coach.ApplicationUserId,
                        FirstName = request.Team.Coach.ApplicationUser.FirstName,
                        LastName = request.Team.Coach.ApplicationUser.LastName
                    },
                    Students = request.Team.Students.Select(student => new StudentViewModel
                    {
                        Id = student.ApplicationUserId,
                        FirstName = student.ApplicationUser.FirstName,
                        LastName = student.ApplicationUser.LastName,
                        UserName = student.ApplicationUser.UserName,
                    }).ToArray()
                }).ToArrayAsync();

            return Ok(result);
        }

        [HttpGet("{id}/teams/granted")]
        public async Task<IActionResult> TeamsGrantedForCompetition(int id)
        {
            var result = await _dbContext.CompetitionJoinRequests
                .Include(request => request.Team)
                .Include(request => request.Competition)
                .Where(request => 
//                    request.Competition.RefereeId == referee.Id && 
                    request.Competition.Id == id && 
                    request.Result == true)
                .Select(request => new TeamViewModel
                {
                    Id = request.Team.Id,
                    Name = request.Team.Name,
                    KindOfSport = request.Team.KindOfSport,
                    Coach = new CoachViewModel
                    {
                        Id = request.Team.Coach.ApplicationUserId,
                        FirstName = request.Team.Coach.ApplicationUser.FirstName,
                        LastName = request.Team.Coach.ApplicationUser.LastName
                    },
                    Students = request.Team.Students.Select(student => new StudentViewModel
                    {
                        Id = student.ApplicationUserId,
                        FirstName = student.ApplicationUser.FirstName,
                        LastName = student.ApplicationUser.LastName,
                        UserName = student.ApplicationUser.UserName,
                    }).ToArray()
                }).ToArrayAsync();

            return Ok(result);
        }

        [HttpGet("soon/{teamId}")]
        [Authorize(Roles = "Coach, Student")]
        public async Task<IEnumerable<CompetitionJoinRequestViewModel>> GetSoonCompetitions([FromRoute] int teamId)
        {
            var result = await _dbContext
                .CompetitionJoinRequests
                .Where(request => request.Result == true && request.Team.Id == teamId && request.Competition.Date > DateTime.Now)
                .Select(request => new CompetitionJoinRequestViewModel
                {
                    Id = request.Id,
                    Competition = new CompetitionViewModel
                    {
                        Id = request.Competition.Id,
                        Name = request.Competition.Name,
                        Place = request.Competition.Place,
                        Date = request.Competition.Date,
                    }
                })
                .ToArrayAsync();

            return result;
        }
    }
}
