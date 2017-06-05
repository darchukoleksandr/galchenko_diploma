using System.Linq;
using System.Threading.Tasks;
using Galchenko.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Galchenko.Controllers
{
    [Route("api/rating")]
    public class RatingController : Controller
    {
        private readonly ApplicationDbContext _dbContext = new ApplicationDbContext();

        [HttpGet("student/{id}")]
        public async Task<double> ByStudent([FromRoute] string id)
        {
            double studentPoints = await _dbContext.CompetitionPoints.Where(points => points.Student.ApplicationUserId == id).SumAsync(points => points.Score);
            double allPoints = await _dbContext.CompetitionPoints.SumAsync(points => points.Score);
            var result = (studentPoints / allPoints) * 100;
            return result;
        }

//        [HttpGet("team/student/{id}")]
//        public async Task<int> BestStudent([FromRoute] string id)
//        {
//            var student = await _dbContext.Students.FirstAsync(student1 => student1.ApplicationUserId == id);
//            var team = _dbContext.Teams.FindAsync(student.TeamId);
//
//            double studentPoints = await _dbContext.CompetitionPoints.Where(points => points.Student.ApplicationUserId == id).SumAsync(points => points.Score);
//            double allPoints = await _dbContext.CompetitionPoints.Where(points => points.Student.TeamId == student.TeamId).SumAsync(points => points.Score);
//            var result = (studentPoints / allPoints) * 100;
//            return (int) result;
//        }
    }
}
