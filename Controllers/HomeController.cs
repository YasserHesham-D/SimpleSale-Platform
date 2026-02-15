using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WoodSalesPlatform.Data;

namespace WoodSalesPlatform.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /
        // GET: /Home
        // GET: /Home/Index
        public async Task<IActionResult> Index()
        {
            var categories = await _context.Categories
                .Include(c => c.Products)
                .ToListAsync();
            
            var featuredProducts = await _context.Products
                .Include(p => p.Category)
                .Where(p => p.IsFeatured)
                .Take(8)
                .ToListAsync();
            
            ViewBag.FeaturedProducts = featuredProducts;
            
            return View(categories);
        }
    }
}
