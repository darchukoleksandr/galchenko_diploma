using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Galchenko.Models.Sports;

namespace Galchenko.Models
{
    public class Competition
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Place { get; set; }
        [Required]
        public DateTime Date { get; set; }
        [Required]
        public int KindOfSportId { get; set; }
        [ForeignKey("KindOfSportId")]
        public KindOfSport KindOfSport { get; set; }
        [Required]
        public string RefereeId { get; set; }
        [ForeignKey("RefereeId")]
        public Referee Referee { get; set; }
        [Required]
        public string ModeratorId { get; set; }
        [ForeignKey("ModeratorId")]
        public Moderator Moderator { get; set; }
    }

    public class CompetitionViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Place { get; set; }
        public DateTime Date { get; set; }
        public KindOfSport KindOfSport { get; set; }
        public RefereeViewModel Referee { get; set; }
        public ModeratorViewModel Moderator { get; set; }
    }

    public class CompetitionJoinRequest
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public Competition Competition { get; set; }
        [Required]
        public Team Team { get; set; }
        public bool? Result { get; set; }
    }

    public class CompetitionJoinRequestViewModel
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public CompetitionViewModel Competition { get; set; }
        [Required]
        public TeamViewModel Team { get; set; }
        public bool? Result { get; set; }
    }

    public class CompetitionJoinRequestResultViewModel
    {
        [Required]
        public int RequestId { get; set; }
        [Required]
        public int CompetitionId { get; set; }
        [Required]
        public int TeamId { get; set; }
        [Required]
        public bool Result { get; set; }
    }

    public class CreateCompetitionViewModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Place { get; set; }
        [Required]
        public DateTime Date { get; set; }
        [Required]
        public int KindOfSportId { get; set; }
        [Required]
        public string RefereeId { get; set; }
        [Required]
        public string ModeratorId { get; set; }
    }

    // -----------------------------------------------------------------------------------------------------

    public class CompetitionPoints
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public Competition Competition { get; set; }
        [Required]
        public Student Student { get; set; }
        public int Score { get; set; }
    }

    public class CompetitionPointsViewModel
    {
        public CompetitionPointsViewModel()
        {
            Competition = new CompetitionViewModel
            {
                Moderator = new ModeratorViewModel(),
                Referee = new RefereeViewModel()
            };
            Student = new StudentViewModel()
            {
                Team = new TeamViewModel()
                {
                    Coach = new CoachViewModel()
                }
            };
        }
        public int Id { get; set; }
        [Required]
        public CompetitionViewModel Competition { get; set; }
        [Required]
        public StudentViewModel Student { get; set; }
        [Required]
        public int Score { get; set; }
    }

//    public class CompetitionResult
//    {
//        public Competition Competition { get; set; }
//        public Team WinnerTeam { get; set; }
//        public Team LoserTeam { get; set; }
//    }


}