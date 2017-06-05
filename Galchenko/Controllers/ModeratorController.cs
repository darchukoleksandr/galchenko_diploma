using System.Linq;
using Galchenko.Data;
using Galchenko.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Galchenko.Controllers
{
    [Route("api/users/moderators")]
    public class ModeratorController : Controller
    {
        private readonly ApplicationDbContext _dbContext = new ApplicationDbContext();

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public ModeratorController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager
        ) {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet("{id}")]
        public ModeratorViewModel ModeratorFullProfile([FromRoute] string id)
        {
            var result = _dbContext.Moderators.Where(moderator => moderator.ApplicationUserId == id)
                .Include(coach => coach.ApplicationUser)
                .Select(coach => new ModeratorViewModel
                {
                    Id = coach.ApplicationUserId,
                    UserName = coach.ApplicationUser.UserName,
                    LastName = coach.ApplicationUser.LastName,
                    FirstName = coach.ApplicationUser.FirstName,
                    Competitions = _dbContext.Competitions
                        .Where(competition => competition.ModeratorId == id)
                        .Select(competition => new CompetitionViewModel
                        {
                            Id = competition.Id,
                            Place = competition.Place,
                            Date = competition.Date,
                            Name = competition.Name,
                            KindOfSport = competition.KindOfSport
                        }).ToArray()
                }).FirstOrDefault();

            return result;
        }


    }
}
