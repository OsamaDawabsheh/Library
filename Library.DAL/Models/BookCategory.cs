using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.DAL.Models
{
    public class BookCategory
    {
        public int BookId { get; set; }
        public Book book { get; set; }

        public int CategoryId { get; set; }

        public Category category { get; set; }

    }
}
