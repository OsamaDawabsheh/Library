using System.ComponentModel.DataAnnotations;

namespace Library.PL.ViewModels
{
    public class ChangePasswordVM
    {

        [Required]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare(nameof(NewPassword))]
        public string ConfirmNewPassword { get; set; }

        public string Email { get; set; }

        public string Token { get; set; }
    }
}
