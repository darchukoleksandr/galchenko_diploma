using System.Linq;
using System.Threading.Tasks;
using Galchenko.Data;
using Galchenko.Models;
using Galchenko.Models.AccountViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Galchenko.Controllers
{
    [Route("api/accounts")]
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _dbContext = new ApplicationDbContext();
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger _logger;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IOptions<IdentityCookieOptions> identityCookieOptions,
            ILoggerFactory loggerFactory)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = loggerFactory.CreateLogger<AccountController>();
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Login([FromBody] LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, false);
                if (result.Succeeded)
                {
                    _logger.LogInformation(1, $"User {model.UserName} logged in.");

                    var user = await _userManager.FindByNameAsync(model.UserName);

                    var userViewModel = RetrieveRoleBasedViewModel(user);

                    return Ok(userViewModel);
                }
                if (result.RequiresTwoFactor)
                {
//                    return RedirectToAction(nameof(SendCode), new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                }
                if (result.IsLockedOut)
                {
                    _logger.LogWarning(2, $"User {model.UserName} locked out.");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return NotFound(model);
                }
            }

            return NotFound(model);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Ok();
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterViewModel model)
        {
            if (!ModelState.IsValid || !new[] {"Student", "Coach", "Referee", "Moderator"}.Contains(model.Role))
                return BadRequest(model);

            var user = new ApplicationUser { UserName = model.UserName, Email = model.Email, FirstName = model.FirstName, LastName = model.LastName};
            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                switch (model.Role)
                {
                    case "Coach":
                        await _dbContext.Coaches.AddAsync(new Coach
                        {
                            ApplicationUserId = user.Id
                        });
                        break;
                    case "Student":
                        await _dbContext.Students.AddAsync(new Student
                        {
                            ApplicationUserId = user.Id
                        });
                        break;
                    case "Referee":
                        await _dbContext.Referees.AddAsync(new Referee
                        {
                            ApplicationUserId = user.Id
                        });
                        break;
                    case "Moderator":
                        await _dbContext.Moderators.AddAsync(new Moderator
                        {
                            ApplicationUserId = user.Id
                        });
                        break;
                }
                await _dbContext.SaveChangesAsync();
                await _userManager.AddToRoleAsync(user, model.Role);
                await _signInManager.SignInAsync(user, isPersistent: false);
                return Ok();
            }

            return BadRequest(model);
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> Current()
        {
            if (_signInManager.IsSignedIn(User))
            {
                var user = await _userManager.FindByNameAsync(User.Identity.Name);

                return Ok(RetrieveRoleBasedViewModel(user));
            }

            return NotFound();
        }

        private async Task<ApplicationUserViewModel> RetrieveRoleBasedViewModel(ApplicationUser user)
        {
            if (await _userManager.IsInRoleAsync(user, "Student"))
            {
                var student = _dbContext.Students.Find(user.Id);
                TeamViewModel team = null;
                if (student.TeamId != null)
                {
                    var dbTeam = _dbContext.Teams.Find(student.TeamId);
                    team = new TeamViewModel
                    {
                        Id = dbTeam.Id,
//                        Coach = dbTeam.Coach.,
                        Name = dbTeam.Name,
                        KindOfSport = dbTeam.KindOfSport
                    };
                }
                return new StudentViewModel
                {
                    Id = user.Id,
                    LastName = user.LastName,
                    FirstName = user.FirstName,
                    UserName = user.UserName,
                    Team = team
                };
            }
            if (await _userManager.IsInRoleAsync(user, "Coach"))
            {
                return (new CoachViewModel
                {

                    Id = user.Id,
                    LastName = user.LastName,
                    FirstName = user.FirstName,
                    UserName = user.UserName,
                });
            }
            if (await _userManager.IsInRoleAsync(user, "Referee"))
            {
                return new RefereeViewModel
                {
                    Id = user.Id,
                    LastName = user.LastName,
                    FirstName = user.FirstName,
                    UserName = user.UserName,
                };
            }
            if (await _userManager.IsInRoleAsync(user, "Moderator"))
            {
                return new ModeratorViewModel
                {
                    Id = user.Id,
                    LastName = user.LastName,
                    FirstName = user.FirstName,
                    UserName = user.UserName
                };
            }

            // MUST NOT HAPPEND!
            return null;
        }
    }
}
