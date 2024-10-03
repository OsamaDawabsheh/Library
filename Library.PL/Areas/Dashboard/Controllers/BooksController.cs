using Microsoft.AspNetCore.Mvc;

namespace Library.PL.Areas.Dashboard.Controllers
{
    public class BooksController : Controller
    {
        [Area("Dashboard")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
