using System.ComponentModel.DataAnnotations;

namespace Library.PL.ViewModels
{
    public class ForgotPasswordVM
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;
    }
}
