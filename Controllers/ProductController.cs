using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjetNet.Data;
using ProjetNet.Models;

namespace ProjetNet.Controllers
{
    public class ProductController : Controller
    {
        private readonly AppDbContext _context;

        public ProductController(AppDbContext context)
        {
            _context = context;
        }

        // Liste tous les produits
        public IActionResult Index()
        {
            var products = _context.Products
                .Include(p => p.Brand)
                .Include(p => p.Category)
                .Include(p => p.SkinType)
                .ToList();
            return View(products);
        }

        // Détails produit
        public IActionResult Details(int id)
        {
            var product = _context.Products
                .Include(p => p.Brand)
                .Include(p => p.Category)
                .Include(p => p.SkinType)
                .Include(p => p.ProductIngredients)
                    .ThenInclude(pi => pi.Ingredient)
                .Include(p => p.Reviews)
                    .ThenInclude(r => r.User)
                .FirstOrDefault(p => p.Id == id);

            if (product == null) return NotFound();
            return View(product);
        }

        // Filtrer produits par type de peau
        public IActionResult FilterBySkinType(int skinTypeId)
        {
            var products = _context.Products
                .Include(p => p.Brand)
                .Include(p => p.Category)
                .Where(p => p.SkinTypeId == skinTypeId)
                .ToList();
            return View("Index", products);
        }

        [HttpGet]
        public IActionResult Search(string query, int? brandId, int? categoryId, int? ingredientId,
                            int? skinTypeId, decimal? minPrice, decimal? maxPrice)
        {
            var products = _context.Products
                .Include(p => p.Brand)
                .Include(p => p.Category)
                .Include(p => p.SkinType)
                .Include(p => p.ProductIngredients)
                .ThenInclude(pi => pi.Ingredient)
                .AsQueryable();

            if (!string.IsNullOrEmpty(query))
                products = products.Where(p => p.Name.Contains(query));

            if (brandId.HasValue)
                products = products.Where(p => p.BrandId == brandId.Value);

            if (categoryId.HasValue)
                products = products.Where(p => p.CategoryId == categoryId.Value);

            if (skinTypeId.HasValue)
                products = products.Where(p => p.SkinTypeId == skinTypeId.Value);

            if (ingredientId.HasValue)
                products = products.Where(p => p.ProductIngredients.Any(pi => pi.IngredientId == ingredientId.Value));

            if (minPrice.HasValue)
                products = products.Where(p => p.Price >= minPrice.Value);

            if (maxPrice.HasValue)
                products = products.Where(p => p.Price <= maxPrice.Value);

            // envoyer les listes pour dropdown
            ViewBag.Brands = _context.Brands.ToList();
            ViewBag.Categories = _context.Categories.ToList();
            ViewBag.Ingredients = _context.Ingredients.ToList();
            ViewBag.SkinTypes = _context.SkinTypes.ToList();

            return View(products.ToList());
        }

    }
}
