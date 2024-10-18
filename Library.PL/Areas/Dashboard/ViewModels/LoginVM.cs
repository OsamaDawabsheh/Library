using System.ComponentModel.DataAnnotations;

namespace Library.PL.Areas.Dashboard.ViewModels
{
    public class LoginVM
    {
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }


    }
}
