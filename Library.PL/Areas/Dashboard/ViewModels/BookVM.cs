using Library.DAL.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Library.PL.Areas.Dashboard.ViewModels
{
    public class BookVM
    {
        public int Id { get; set; }
        public string? Img { get; set; }
        public IFormFile Image { get; set; }
        public string? File { get; set; }
        public IFormFile BookFile { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string ISBN { get; set; } = null!;
        public string Author { get; set; } = null!;
        public DateTime PublicationDate { get; set; }
        public bool IsDeleted { get; set; }

        public ICollection<Category> categories { get; set; } = new List<Category>();

        public ICollection<int> categoriesId { get; set; } = null!;

        public List<SelectListItem>? selectLists { get; set; }
        public List<BookCategory>? BookCategories { get; set; }

    }
}
