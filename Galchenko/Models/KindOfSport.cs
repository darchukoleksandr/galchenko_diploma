using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Galchenko.Models.Sports
{
    public class KindOfSport
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string Sport { get; set; }
    }

    public class CreateKindOfSportViewModel
    {
        public string Sport { get; set; }
    }
}
