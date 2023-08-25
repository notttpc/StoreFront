using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StoreFront.DATA.EF.Models;
using StoreFront.UI.MVC.Models;
using System.Diagnostics;

namespace StoreFront.UI.MVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly AnimeShopContext _context;

        public HomeController(ILogger<HomeController> logger, AnimeShopContext context)
        {
            _logger = logger;
            _context = context;
        }


        public IActionResult Index()
        {
            var animeShopContext = _context.Products.Where(p => p.IsFeatured)
                .Include(p => p.Category).Include(p => p.Company).Include(p => p.Genre).Include(p => p.ProductStatus).Include(p => p.Sword);
            return View(animeShopContext.ToList());
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}