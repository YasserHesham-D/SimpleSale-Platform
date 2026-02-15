using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WoodSalesPlatform.Data;
using WoodSalesPlatform.Models;

namespace WoodSalesPlatform.Controllers
{
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProductController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /Product/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var product = await _context.Products
                .Include(p => p.Category)
                .Include(p => p.ProductImages)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: /Product/PlaceOrder
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PlaceOrder(Order order)
        {
            var product = await _context.Products.FindAsync(order.ProductId);
            
            if (product == null || product.Stock < order.Quantity)
            {
                TempData["Error"] = "Insufficient stock or product not found";
                return RedirectToAction("Details", new { id = order.ProductId });
            }

            order.OrderDate = DateTime.Now;
            order.TotalPrice = product.Price * order.Quantity;
            
            product.Stock -= order.Quantity;
            
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            return RedirectToAction("OrderSuccess");
        }

        // GET: /Product/OrderSuccess
        public IActionResult OrderSuccess()
        {
            return View();
        }
    }
}
