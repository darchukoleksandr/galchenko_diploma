using System.ComponentModel.DataAnnotations;

namespace Galchenko.Models
{
    public abstract class ApplicationUserViewModel
    {
        [Required]
        public string Id { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        public virtual string Role { get; set; } = "";
    }
    
    public class CreateRefereeViewModel
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
    }

    public class StudentViewModel : ApplicationUserViewModel
    {
        public TeamViewModel Team { get; set; }
        public double Rating { get; set; }
        public override string Role { get; set; } = "Student";
    }
    
    public class RefereeViewModel : ApplicationUserViewModel
    {
        public override string Role { get; set; } = "Referee";
    }

    public class CoachViewModel : ApplicationUserViewModel
    {
        public TeamViewModel Team{ get; set; }
        public override string Role { get; set; } = "Coach";
    }
    public class ModeratorViewModel : ApplicationUserViewModel
    {
        public override string Role { get; set; } = "Moderator";
        public CompetitionViewModel[] Competitions { get; set; }
    }
}

