using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.DAL.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Img { get; set; }
        public bool IsDeleted { get; set; }
        public ICollection<Book> books { get; set; } = null!;
        public List<BookCategory>? BookCategories { get; set; }

    }
}
