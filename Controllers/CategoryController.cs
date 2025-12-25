using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjetNet.Data;
using ProjetNet.Models;

namespace ProjetNet.Controllers
{
    public class CategoryController : Controller
    {
        private readonly AppDbContext _context;

        public CategoryController(AppDbContext context)
        {
            _context = context;
        }

        // Liste des catégories
        public IActionResult Index()
        {
            var categories = _context.Categories.ToList();
            return View(categories);
        }

        // Produits par catégorie
        public IActionResult ProductsByCategory(int id)
        {
            var products = _context.Products
                .Include(p => p.Brand)
                .Include(p => p.SkinType)
                .Where(p => p.CategoryId == id)
                .ToList();
            return View("Index", products);
        }
    }
}
