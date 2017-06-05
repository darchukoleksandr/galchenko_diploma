using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Galchenko.Data;
using Galchenko.Models.Sports;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;

namespace Galchenko.Controllers
{
    [Route("api/sports")]
    public class SportsController : Controller
    {
        private readonly ApplicationDbContext _dbContext = new ApplicationDbContext();

        [HttpPost("create")]
        [Authorize(Roles = "Moderator")]
        public async Task<IActionResult> Create([FromBody] KindOfSport sport)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            await _dbContext.KindOfSports.AddAsync(sport);
            await _dbContext.SaveChangesAsync();

            return Ok();
        }

        [HttpPost("delete")]
        [Authorize(Roles = "Moderator")]
        public async Task<IActionResult> Delete([FromBody] KindOfSport sport)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var result = _dbContext.KindOfSports.Find(sport.Id);
            if (result == null)
            {
                return NotFound("No such kind of sport in database!");
            }
            try
            {
                _dbContext.KindOfSports.Remove(result);
                await _dbContext.SaveChangesAsync();
            }
            catch
            {
                return StatusCode(203);
            }


            return Ok();
        }

        [HttpPost("update")]
        [Authorize(Roles = "Moderator")]
        public async Task<IActionResult> Update([FromBody] KindOfSport sport)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var result = _dbContext.KindOfSports.Find(sport.Id);
            if (result == null)
            {
                return NotFound("No such kind of sport in database!");
            }

            result.Sport = sport.Sport;

            await _dbContext.SaveChangesAsync();

            return Ok();
        }

        [HttpGet("all")]
        public IEnumerable<KindOfSport> All()
        {
            return _dbContext.KindOfSports.ToArray();
        }
    }
}
