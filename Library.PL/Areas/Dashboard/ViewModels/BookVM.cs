using Library.DAL.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Library.PL.Areas.Dashboard.ViewModels
{
    public class BookVM
    {
        public int Id { get; set; }
        public string? Img { get; set; }

        [Required]
        public IFormFile Image { get; set; }
        public string? File { get; set; }

        [Required]
        public IFormFile BookFile { get; set; }

        [Required]
        public string Title { get; set; } = null!;

        [Required]
        public string Description { get; set; } = null!;

        [Required]
        public string ISBN { get; set; } = null!;

        [Required]
        public string Author { get; set; } = null!;

        [Required]
        public DateTime PublicationDate { get; set; }
        public bool IsDeleted { get; set; }

        public ICollection<Category> categories { get; set; } = new List<Category>();

        public ICollection<int> categoriesId { get; set; } = null!;

        public List<SelectListItem>? selectLists { get; set; }
        public List<BookCategory>? BookCategories { get; set; }

    }
}
