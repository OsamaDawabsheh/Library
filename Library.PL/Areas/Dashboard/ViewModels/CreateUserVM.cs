using System.ComponentModel.DataAnnotations;

namespace Library.PL.Areas.Dashboard.ViewModels
{
    public class CreateUserVM
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }
        public string PhoneNumber { get; set; }

        public string? Img { get; set; }
        public IFormFile Image { get; set; }

        public string Address { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? MemberDate { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public bool IsDeleted { get; set; }

        public IEnumerable<string>? Roles { get; set; }

    }
}
