using System.Collections.Generic;
using System.Linq;
using Galchenko.Data;
using Galchenko.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Galchenko.Controllers
{
    [Route("api/users/referees")]
    public class RefereeController : Controller
    {
        private readonly ApplicationDbContext _dbContext = new ApplicationDbContext();

        [HttpGet("all")]
        public IEnumerable<RefereeViewModel> All()
        {
            return _dbContext.Referees
//                .Include(referee => referee.KindOfSport)
                .Include(referee => referee.ApplicationUser)
                .Select(referee => new RefereeViewModel
                {
                    Id = referee.ApplicationUserId,
                    FirstName = referee.ApplicationUser.FirstName,
                    LastName = referee.ApplicationUser.LastName,
                    UserName = referee.ApplicationUser.Email,
//                    KindOfSport = referee.KindOfSport
                })
                .ToArray();
        }

//        [HttpGet("sport")]
//        public IEnumerable<RefereeViewModel> BySportType([FromBody] KindOfSport sport)
//        {
//            return _dbContext.Referees
//                .Include(referee => referee.KindOfSport)
//                .Include(referee => referee.ApplicationUser)
//                .Where(referee => referee.KindOfSport == sport)
//                .Select(referee => new RefereeViewModel
//                {
//                    Id = referee.ApplicationUserId,
//                    FirstName = referee.ApplicationUser.FirstName,
//                    LastName = referee.ApplicationUser.LastName,
//                    UserName = referee.ApplicationUser.Email,
//                    KindOfSport = referee.KindOfSport
//                })
//                .ToArray();
//        }
    }
}
