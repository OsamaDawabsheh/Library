using AutoMapper;
using Library.DAL.Data;
using Library.PL.Areas.Dashboard.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Library.PL.Controllers
{
    public class BooksController : Controller
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public BooksController(ApplicationDbContext context,IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }
        public IActionResult Index(int page = 1, int pageSize = 20)
        {
            ViewData["BookCategories"] = context.BookCategories.ToList();
            ViewData["Categories"] = context.Categories.ToList();

            var books = context.Books.OrderBy(b => b.Title).Skip((page - 1) * pageSize).Take(pageSize).ToList();

            var totalBooks = context.Books.Count();
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalBooks / pageSize);
            ViewBag.CurrentPage = page;

            return View(mapper.Map<IEnumerable<BookVM>>(books));
        }

        public IActionResult Details(int id)
        {

            ViewData["BookCategories"] = context.BookCategories.ToList();
            var book = context.Books.Include(b => b.categories).FirstOrDefault(b => b.Id == id);
            if (book == null) { 
                return NotFound();
            }
            return View(mapper.Map<BookVM>(book));
        }
    }
}
