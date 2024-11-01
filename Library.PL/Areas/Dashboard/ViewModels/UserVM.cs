using Library.DAL.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Library.PL.Areas.Dashboard.ViewModels
{
    public class UserVM
    {
        public string Id { get; set; }

        [Required]

        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]

        public string PhoneNumber { get; set; }

        public string? Img { get; set; }

        public IFormFile Image { get; set; }

        [Required]
        public string Address { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? MemberDate { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public bool IsDeleted { get; set; }

        public IEnumerable<string> Roles { get; set; }



    }
}
