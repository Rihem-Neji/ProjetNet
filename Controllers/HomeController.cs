using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjetNet.Data;
using ProjetNet.Models;
using ProjetNet.Models.ViewModel;

namespace ProjetNet.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index(Dictionary<string, int?> categoryPages = null)
        {
            var categories = _context.Categories.Include(c => c.Products).ThenInclude(p => p.Brand).ToList();

            // Si categoryPages est null, crée un dictionnaire vide
            categoryPages ??= new Dictionary<string, int?>();

            var pageSize = 4; // 4 produits par page

            var model = categories.Select(c =>
            {
                var key = $"category{c.Id}Page";
                var page = categoryPages.ContainsKey(key) ? categoryPages[key] ?? 1 : 1;

                var totalProducts = c.Products.Count;
                var totalPages = (int)Math.Ceiling((double)totalProducts / pageSize);

                var productsToShow = c.Products
                    .OrderBy(p => p.Id)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();

                return new CategoryPaginationViewModel
                {
                    Category = c,
                    Page = page,
                    TotalPages = totalPages,
                    Products = productsToShow
                };
            }).ToList();

            return View(model);
        }


    

   public IActionResult ProductsByCategory(int categoryId, int page = 1)
        {
            int pageSize = 3; // 3 produits par page
            var category = _context.Categories
                .Include(c => c.Products)
                .ThenInclude(p => p.Brand)
                .FirstOrDefault(c => c.Id == categoryId);

            if (category == null) return NotFound();

            var products = category.Products
                .OrderBy(p => p.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var model = new CategoryPaginationViewModel
            {
                Category = category,
                Page = page,
                TotalPages = (int)Math.Ceiling((double)category.Products.Count / pageSize),
                Products = products
            };

            return PartialView("_CategoryProducts", model);
        }

    }
}
