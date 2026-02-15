using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WoodSalesPlatform.Data;
using WoodSalesPlatform.Models;
using WoodSalesPlatform.Services;

namespace WoodSalesPlatform.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly JwtService _jwtService;
        private readonly IWebHostEnvironment _environment;

        public AdminController(ApplicationDbContext context, JwtService jwtService, IWebHostEnvironment environment)
        {
            _context = context;
            _jwtService = jwtService;
            _environment = environment;
        }

        // GET: /Admin/Login
        public IActionResult Login()
        {
            // If already logged in, redirect to dashboard
            if (IsAuthenticated())
                return RedirectToAction("Dashboard");

            return View();
        }

        // POST: /Admin/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string username, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);

            if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
            {
                var hash = BCrypt.Net.BCrypt.HashPassword("admin123");
                Console.WriteLine(hash);

                ViewBag.Error = "Invalid credentials";
                return View();
            }

            var token = _jwtService.GenerateToken(user.Username, user.Id);
            
            Response.Cookies.Append("AuthToken", token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.Now.AddHours(24)
            });

            return RedirectToAction("Dashboard");
        }

        // GET: /Admin/Logout
        public IActionResult Logout()
        {
            Response.Cookies.Delete("AuthToken");
            return RedirectToAction("Index", "Home");
        }

        // GET: /Admin/Dashboard
        public async Task<IActionResult> Dashboard()
        {
            if (!IsAuthenticated())
                return RedirectToAction("Login");

            var orders = await _context.Orders
                .Include(o => o.Product)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();

            return View(orders);
        }

        // GET: /Admin/ManageProducts
        public async Task<IActionResult> ManageProducts()
        {
            if (!IsAuthenticated())
                return RedirectToAction("Login");

            var products = await _context.Products
                .Include(p => p.Category)
                .ToListAsync();

            ViewBag.Categories = await _context.Categories.ToListAsync();
            return View(products);
        }

        // POST: /Admin/AddProduct
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddProduct(Product product, IFormFile mainImage, List<IFormFile> additionalImages)
        {
            if (!IsAuthenticated())
                return RedirectToAction("Login");

            // Handle main image upload
            if (mainImage != null && mainImage.Length > 0)
            {
                var mainImagePath = await SaveImageAsync(mainImage);
                product.MainImage = mainImagePath;
            }

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            // Handle additional images
            if (additionalImages != null && additionalImages.Any())
            {
                foreach (var image in additionalImages)
                {
                    if (image != null && image.Length > 0)
                    {
                        var imagePath = await SaveImageAsync(image);
                        var productImage = new ProductImage
                        {
                            ProductId = product.Id,
                            ImageUrl = imagePath,
                            IsPrimary = false
                        };
                        _context.ProductImages.Add(productImage);
                    }
                }
                await _context.SaveChangesAsync();
            }

            TempData["Success"] = "Product added successfully!";
            return RedirectToAction("ManageProducts");
        }

        // POST: /Admin/DeleteProduct
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            if (!IsAuthenticated())
                return RedirectToAction("Login");

            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
               // await _context.SaveChangesAsync();
                TempData["Success"] = "Product deleted successfully!";
            }

            return RedirectToAction("ManageProducts");
        }

        // GET: /Admin/ManageCategories
        public async Task<IActionResult> ManageCategories()
        {
            if (!IsAuthenticated())
                return RedirectToAction("Login");

            var categories = await _context.Categories.ToListAsync();
            return View(categories);
        }


        [HttpGet]
        public  IActionResult GetCategoryCount(int id)
        {
            if (!IsAuthenticated())
                return RedirectToAction("Login");

            var count = _context.Products.Count(p => p.CategoryId == id);
            return Json(new { count });
        }

        public class CategoryViewModel
        {
            public int ProductCount { get; set; }
        }
        // POST: /Admin/AddCategory
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddCategory(Category category, IFormFile categoryImage)
        {
            if (!IsAuthenticated())
                return RedirectToAction("Login");

            // Handle category image upload
            if (categoryImage != null && categoryImage.Length > 0)
            {
                var imagePath = await SaveImageAsync(categoryImage);
                category.ImageUrl = imagePath;
            }

            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Category added successfully!";
            return RedirectToAction("ManageCategories");
        }

        // POST: /Admin/DeleteCategory
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            if (!IsAuthenticated())
                return RedirectToAction("Login");

            var category = await _context.Categories.FindAsync(id);
            if (category != null)
            {
                _context.Categories.Remove(category);
              //  await _context.SaveChangesAsync();
                TempData["Success"] = "Category deleted successfully!";
            }

            return RedirectToAction("ManageCategories");
        }

        // POST: /Admin/ToggleFeatured
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleFeatured(int id)
        {
            if (!IsAuthenticated())
                return RedirectToAction("Login");

            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                product.IsFeatured = !product.IsFeatured;
                await _context.SaveChangesAsync();
                TempData["Success"] = product.IsFeatured ? "Product added to featured!" : "Product removed from featured!";
            }

            return RedirectToAction("ManageProducts");
        }

        // POST: /Admin/ToggleDone
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleDone(int id)
        {
            if (!IsAuthenticated())
                return RedirectToAction("Login");

            var order = await _context.Orders.FindAsync(id);
            if (order != null)
            {
                order.Done = !order.Done;
                await _context.SaveChangesAsync();
                TempData["Success"] = order.Done ? "Order Done " : "Something Went Wrong";
            }

            return RedirectToAction("DashBoard");
        }
        // Helper method to save uploaded images
        private async Task<string> SaveImageAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return string.Empty;

            // Create uploads directory if it doesn't exist
            var uploadsPath = Path.Combine(_environment.WebRootPath, "uploads");
            if (!Directory.Exists(uploadsPath))
            {
                Directory.CreateDirectory(uploadsPath);
            }

            // Generate unique filename
            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            var filePath = Path.Combine(uploadsPath, fileName);

            // Save file
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            // Return relative path
            return "/uploads/" + fileName;
        }

        private bool IsAuthenticated()
        {
            return Request.Cookies.ContainsKey("AuthToken");
        }
    }
}
