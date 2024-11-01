using AutoMapper;
using Library.DAL.Data;
using Library.PL.Areas.Dashboard.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Drawing.Printing;

namespace Library.PL.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public CategoriesController(ApplicationDbContext context,IMapper mapper) {
            this.context = context;
            this.mapper = mapper;
        }
        public IActionResult Index(int page = 1, int pageSize = 20)
        {
            var categories = context.Categories.OrderBy(b => b.Name).Skip((page - 1) * pageSize).Take(pageSize).ToList();

            var totalCategories = context.Categories.Count();
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalCategories / pageSize);
            ViewBag.CurrentPage = page;

            return View(mapper.Map<IEnumerable<CategoryVM>>(categories));
        }

        public IActionResult CategoryBooks(int id, int page = 1, int pageSize = 20)
        {
            var books = context.BookCategories.Where(bc => bc.CategoryId == id).Select(bc => bc.book).OrderBy(b => b.Title).Skip((page - 1) * pageSize).Take(pageSize).ToList();
            var totalBooks = context.BookCategories.Where(bc => bc.CategoryId == id).Select(bc => bc.book).Count();
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalBooks / pageSize);
            ViewBag.CurrentPage = page;

            ViewData["Category"] = context.Categories.FirstOrDefault(c => c.Id == id);

            if (books == null)
            {
                return NotFound();
            }
            return View(mapper.Map< IEnumerable<BookVM>>(books));
        }
    }
}
