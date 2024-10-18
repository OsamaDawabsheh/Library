using AutoMapper;
using Library.DAL.Data;
using Library.DAL.Models;
using Library.PL.Areas.Dashboard.ViewModels;
using Library.PL.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NuGet.Packaging;
using NuGet.Versioning;
using System.Net;

namespace Library.PL.Areas.Dashboard.Controllers
{
    [Area("Dashboard")]

    public class BooksController : Controller
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public BooksController(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public IActionResult Index()
        {
            ViewData["BookCategories"] = context.BookCategories.ToList();
            ViewData["Categorise"] = context.Categories.ToList();

            return View(mapper.Map<IEnumerable<BookVM>>(context.Books.ToList()));
        }


        [HttpGet]
        public IActionResult Create()
        {
            var model = new BookVM
            {
                selectLists = context.Categories.Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Name
                }).ToList()
            };

            
            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(BookVM model)
        {

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (model.categoriesId != null)
            {
                foreach (var item in model.categoriesId)
                {
                    var category = context.Categories.Find(item);
                    if (category != null)
                    {
                        model.categories.Add(category);
                    }
                }
            }
    
            model.Img = FilesSettings.UploadFile(model.Image, "images");
            model.File = FilesSettings.UploadFile(model.BookFile, "books");
            var book = mapper.Map<Book>(model);

            context.Add(book);
            context.SaveChanges();
            return RedirectToAction("Index");
        }


        [HttpGet]
        public IActionResult Edit(int id)
        {
            var book =  context.Books
                .Include(b => b.categories)
                 .FirstOrDefault(b => b.Id == id);

            var allCategories = context.Categories.ToList();

            var selectedCategories =  context.BookCategories
                .Where(bc => bc.BookId == id)
                .Select(bc => bc.CategoryId) 
                .ToList();


             var bookVM = mapper.Map<BookVM>(book);


            bookVM.selectLists = allCategories.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Name,
                Selected = selectedCategories.Contains(c.Id) 
            }).ToList();

            if (bookVM is null)
            {
                return NotFound();
            }


            return View(bookVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(BookVM model)
        {

            if (model.Image is null)
            {
                ModelState.Remove("Image");
            }
            else
            {
                FilesSettings.DeleteFile(model.Img, "images");
                model.Img = FilesSettings.UploadFile(model.Image, "images");
            }
            if (model.BookFile is null)
            {
                ModelState.Remove("BookFile");
            }
            else
            {
                FilesSettings.DeleteFile(model.File, "books");
                model.File = FilesSettings.UploadFile(model.BookFile, "books");
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

       var book = context.Books
       .Include(b => b.categories) 
       .FirstOrDefault(b => b.Id == model.Id);

            if (book == null)
            {
                return NotFound();
            }
            book.categories.Clear();

            if (model.categoriesId != null)
            {
                foreach (var item in model.categoriesId)
                {
                    var category = context.Categories.Find(item);
                    if (category != null)
                    {
                        model.categories.Add(category);
                    }
                }
            }

            mapper.Map(model, book);


            context.SaveChanges();

            return RedirectToAction(nameof(Index));
            
        }

        [HttpGet]

        public IActionResult Details(int id)
        {
            ViewData["BookCategories"] = context.BookCategories.ToList();

            var book = context.Books.Include(b => b.categories).FirstOrDefault(b => b.Id == id);

            return View(mapper.Map<BookVM>(book));
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var book = context.Books.Find(id);
            if (book is null)
            {
                return NotFound();
            }

            return View(mapper.Map<BookVM>(book));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(BookVM model)
        {
            var book = context.Books.Find(model.Id);

            if (book is null)
            {
                return RedirectToAction(nameof(Index));
            }

            FilesSettings.DeleteFile(book.Img, "images");
            FilesSettings.DeleteFile(book.File, "books");

            context.Books.Remove(book);

            context.SaveChanges();


            return RedirectToAction(nameof(Index));

        }
    }
}
