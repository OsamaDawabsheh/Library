using System.ComponentModel.DataAnnotations;

namespace Library.PL.ViewModels
{
    public class RegisterVM
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }

        [Required]
        public string PhoneNumber { get; set; }

        public string? Img { get; set; }

        [Required]
        public IFormFile Image { get; set; }

        [Required]
        public string Address { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? MemberDate { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public bool IsDeleted { get; set; }

        public IEnumerable<string>? Roles { get; set; }

    }
}
