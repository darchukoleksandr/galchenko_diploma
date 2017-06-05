using System.ComponentModel.DataAnnotations;

namespace Galchenko.Models.AccountViewModels
{
    public class LoginViewModel
    {
//        [Required]
//        [EmailAddress]
//        public string Email { get; set; }
        [Required]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        
        public bool RememberMe { get; set; }
    }
}
