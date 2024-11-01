using Library.DAL.Models;
using System.ComponentModel.DataAnnotations;

namespace Library.PL.Areas.Dashboard.ViewModels
{
    public class CategoryVM
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = null!;
        public string? Img { get; set; }
        [Required]
        public IFormFile Image { get; set; }
        public bool IsDeleted { get; set; }
        public ICollection<Book>? books { get; set; } = null!;

        public List<BookCategory>? BookCategories { get; set; }

    }

}
