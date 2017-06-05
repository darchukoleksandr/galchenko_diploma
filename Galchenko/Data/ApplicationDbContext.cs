using Galchenko.Models;
using Galchenko.Models.Sports;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace Galchenko.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connection = new SqliteConnection("Filename=./database.sqlite");

            optionsBuilder.UseSqlite(connection);
        }

        public DbSet<KindOfSport> KindOfSports { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<Competition> Competitions { get; set; }

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Referee> Referees { get; set; }
        public DbSet<Coach> Coaches { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Moderator> Moderators { get; set; }

        public DbSet<CompetitionPoints> CompetitionPoints { get; set; }

        public DbSet<TeamJoinRequest> TeamJoinRequests { get; set; }
        public DbSet<CompetitionJoinRequest> CompetitionJoinRequests { get; set; }
    }
}
