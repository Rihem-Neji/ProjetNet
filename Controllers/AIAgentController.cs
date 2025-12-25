using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjetNet.Data;
using ProjetNet.Models;
using ProjetNet.Models.ViewModel;
using ProjetNet.Services;

namespace ProjetNet.Controllers
{
    public class AIAgentController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IGeminiAIService _geminiService;

        public AIAgentController(AppDbContext context, IGeminiAIService geminiService)
        {
            _context = context;
            _geminiService = geminiService;
        }

        // GET: /AIAgent/Index
        public IActionResult Index()
        {
            // Charger les types de peau pour le dropdown
            ViewBag.SkinTypes = _context.SkinTypes.ToList();
            return View();
        }

        // POST: /AIAgent/Recommend
        [HttpPost]
        public async Task<IActionResult> Recommend(AIAgentRequestViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.SkinTypes = _context.SkinTypes.ToList();
                return View("Index", model);
            }

            // 1. Récupérer le type de peau sélectionné
            var skinType = _context.SkinTypes.Find(model.SkinTypeId);
            if (skinType == null)
            {
                ModelState.AddModelError("", "Type de peau non valide.");
                ViewBag.SkinTypes = _context.SkinTypes.ToList();
                return View("Index", model);
            }

            // 2. Récupérer tous les produits avec leurs relations
            var allProducts = _context.Products
                .Include(p => p.Brand)
                .Include(p => p.Category)
                .Include(p => p.SkinType)
                .Include(p => p.ProductIngredients)
                    .ThenInclude(pi => pi.Ingredient)
                .ToList();

            // 3. Filtrer les produits par type de peau
            var suitableProducts = allProducts
                .Where(p => p.SkinTypeId == model.SkinTypeId)
                .ToList();

            // 4. Si allergies, exclure les produits contenant ces ingrédients
            if (!string.IsNullOrEmpty(model.Allergies))
            {
                var allergyKeywords = model.Allergies.Split(',')
                    .Select(a => a.Trim().ToLower())
                    .ToList();

                suitableProducts = suitableProducts
                    .Where(p => !p.ProductIngredients.Any(pi =>
                        allergyKeywords.Any(k => pi.Ingredient != null && pi.Ingredient.Name.ToLower().Contains(k))
                    ))
                    .ToList();
            }

            // 5. Appeler Gemini AI pour obtenir la recommandation
            string aiRecommendation = await _geminiService.GetProductRecommendationsAsync(
                skinType.Type,
                model.Allergies,
                model.AdditionalConcerns,    // <-- mis à jour ici
                suitableProducts
            );

            // 6. Parser la réponse AI pour extraire les noms de produits recommandés
            var recommendedProducts = ExtractRecommendedProducts(aiRecommendation, allProducts);

            // 7. Créer le ViewModel de résultat
            var result = new AIAgentResponseViewModel
            {
                AIRecommendation = aiRecommendation,
                RecommendedProducts = recommendedProducts,
                UserRequest = model
            };

            return View("Result", result);
        }

        private List<Product> ExtractRecommendedProducts(string aiText, List<Product> allProducts)
        {
            var recommended = new List<Product>();

            // Rechercher les noms de produits dans le texte AI
            foreach (var product in allProducts)
            {
                if (!string.IsNullOrEmpty(product.Name) &&
                    aiText.Contains(product.Name, StringComparison.OrdinalIgnoreCase))
                {
                    recommended.Add(product);
                }
            }

            return recommended.Take(5).ToList(); // Max 5 produits
        }
    }
}
