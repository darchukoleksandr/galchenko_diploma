using System.ComponentModel.DataAnnotations;

namespace Galdiplom.Models.AccountViewModels
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
