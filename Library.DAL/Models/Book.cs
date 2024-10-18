using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.DAL.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string Img { get; set; } = null!;
        public string File { get; set; } = null!;
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string ISBN { get; set; } = null!;
        public string Author { get; set; } = null!;
        public DateTime PublicationDate { get; set; }
        public bool IsDeleted { get; set; }
        public ICollection<Category> categories { get; set; } = null!;

        public List<BookCategory>? BookCategories { get; set; } 


    }
}
