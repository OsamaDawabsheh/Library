using AutoMapper;
using Library.DAL.Data;
using Library.DAL.Models;
using Library.PL.Areas.Dashboard.ViewModels;
using Library.PL.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Library.PL.Areas.Dashboard.Controllers
{
    [Area("Dashboard")]
    [Authorize(Roles = "Admin")]

    public class CategoriesController : Controller
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public CategoriesController(ApplicationDbContext context , IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }


        public IActionResult Index()
        {
            return View(mapper.Map<IEnumerable<CategoryVM>>(context.Categories.ToList()));
        }


        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CategoryVM model)
        {

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            model.Img = FilesSettings.UploadFile(model.Image, "categories");

            var category = mapper.Map<Category>(model);

            context.Add(category);
            context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var category = context.Categories.Find(id);
            if (category is null)
            {
                return NotFound();
            }
           
            return View(mapper.Map<CategoryVM>(category));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(CategoryVM model)
        {
            if (model.Image is null)
            {
                ModelState.Remove("image");

            }
            else
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                FilesSettings.DeleteFile(model.Img, "categories");
                model.Img = FilesSettings.UploadFile(model.Image, "categories");
            }
            
           


           
            var category = context.Categories.Find(model.Id);

            if (category is null)
            {
                return NotFound();
            }

            mapper.Map(model, category);

            context.SaveChanges();

            return RedirectToAction(nameof(Index));

        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var category = context.Categories.Find(id);
            if (category is null)
            {
                return NotFound();
            }

            return View(mapper.Map<CategoryVM>(category));
        }

        public IActionResult DeleteConfirm(int id)
        {
            var category = context.Categories.Find(id);

            if (category is null)
            {
                return RedirectToAction(nameof(Index));
            }

            FilesSettings.DeleteFile(category.Img, "categories");
            context.Categories.Remove(category);

            context.SaveChanges();

            return Ok(new { msg = "The category deleted successfully" });

        }

    }
}
