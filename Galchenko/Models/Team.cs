using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Galchenko.Models.Sports;

namespace Galchenko.Models
{
    public class Team
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public int KindOfSportId { get; set; }
        [ForeignKey("KindOfSportId")]
        public KindOfSport KindOfSport { get; set; }
        public string CoachId { get; set; }
        [ForeignKey("CoachId")]
        public Coach Coach { get; set; }
        public ICollection<Student> Students { get; set; }
    }

    public class TeamViewModel
    {
        public TeamViewModel()
        {
//            Id = 0;
            Name = "";
            Students = new StudentViewModel[0];
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public KindOfSport KindOfSport { get; set; }
        public CoachViewModel Coach { get; set; }
        public StudentViewModel[] Students { get; set; }
        public int Rating { get; set; }
    }

    public class TeamJoinRequest
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public Student Student { get; set; }
        [Required]
        public Team Team { get; set; }
        public bool? Result { get; set; }
    }

    public class TeamJoinRequestViewModel
    {
        public int Id { get; set; }
        public StudentViewModel Student { get; set; }
        public TeamViewModel Team { get; set; }
        public bool? Result { get; set; }
    }

    public class TeamJoinRequestResultViewModel
    {
        [Required]
        public int TeamJoinRequestId { get; set; }
        [Required]
        public int TeamId { get; set; }
        [Required]
        public string StudentId { get; set; }
        [Required]
        public bool Result { get; set; }
    }

    public class CreateTeamViewModel
    {
        public string Name { get; set; }
        public int KindOfSportId { get; set; }
        public string CoachId { get; set; }
    }
}
