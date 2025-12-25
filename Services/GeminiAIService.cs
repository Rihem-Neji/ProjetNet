using ProjetNet.Models;
using System.Text.Json;
using System.Net.Http.Json;

namespace ProjetNet.Services
{
    public interface IGeminiAIService
    {
        Task<string> GetProductRecommendationsAsync(
            string skinType,
            string allergies,
            string preoccupations,
            List<Product> availableProducts
        );
    }

    public class GeminiAIService : IGeminiAIService
    {
        private readonly string _apiKey;
        private readonly string _model;
        private readonly HttpClient _httpClient;

        public GeminiAIService(IConfiguration configuration, HttpClient httpClient)
        {
            _apiKey = configuration["GeminiAI:ApiKey"]?.Trim();
            _model = configuration["GeminiAI:Model"]?.Trim() ?? "gemini-1.5-flash";
            _httpClient = httpClient;

            if (string.IsNullOrEmpty(_apiKey))
            {
                throw new ArgumentNullException(
                    "GeminiAI:ApiKey",
                    "La clé API Gemini est manquante."
                );
            }
        }

        public async Task<string> GetProductRecommendationsAsync(
            string skinType,
            string allergies,
            string additionalConcerns,
            List<Product> availableProducts)
        {
            var prompt = BuildPrompt(skinType, allergies, additionalConcerns, availableProducts);
            return await CallGeminiAPIAsync(prompt);
        }

        private string BuildPrompt(
            string skinType,
            string allergies,
            string additionalConcerns,
            List<Product> products)
        {
            var productList = string.Join("\n", products.Select(p =>
                $"- {p.Name} ({p.Brand?.Name ?? "Inconnue"}) - " +
                $"{p.Category?.Name ?? "Inconnue"} - " +
                $"Prix: {p.Price} DT - " +
                $"Ingrédients: {string.Join(", ", p.ProductIngredients?.Select(pi => pi.Ingredient?.Name) ?? new List<string>())}"
            ));

            return $@"
Tu es un expert en cosmétique pour la boutique 'Dreamskin'.

Client :
- Type de peau : {skinType}
- Allergies / À éviter : {allergies ?? "Aucune"}
- Préoccupations majeures : {additionalConcerns}

Produits en stock :
{productList}

Mission :
Recommande un pack de 3 produits de cette liste uniquement.

IMPORTANT :
- Utilise le NOM EXACT
- Évite les allergènes
- Explique brièvement pourquoi

Format :
## Pack Recommandé
1. [Nom Exact] : [Raison]
2. [Nom Exact] : [Raison]
3. [Nom Exact] : [Raison]

Routine : [Matin / Soir]
";
        }

        private async Task<string> CallGeminiAPIAsync(string prompt)
        {
            var requestBody = new
            {
                contents = new[]
                {
                    new
                    {
                        parts = new[]
                        {
                            new { text = prompt }
                        }
                    }
                }
            };

            string url = $"https://generativelanguage.googleapis.com/v1beta/models/{_model}:generateContent?key={_apiKey}";

            try
            {
                var response = await _httpClient.PostAsJsonAsync(url, requestBody);
                var rawJson = await response.Content.ReadAsStringAsync();

                Console.WriteLine("===== RAW GEMINI RESPONSE =====");
                Console.WriteLine(rawJson);
                Console.WriteLine("================================");

                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"[GeminiAI Error] Status: {response.StatusCode}");
                    return GetFallbackRecommendation(prompt);
                }

                using var doc = JsonDocument.Parse(rawJson);
                var text = doc.RootElement
                    .GetProperty("candidates")[0]
                    .GetProperty("content")
                    .GetProperty("parts")[0]
                    .GetProperty("text")
                    .GetString();

                return string.IsNullOrWhiteSpace(text) ? "Désolé, la réponse Gemini est vide." : text;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[GeminiAI Exception] {ex.Message}");
                return GetFallbackRecommendation(prompt);
            }
        }

        private string GetFallbackRecommendation(string prompt)
        {
            return
                "## Pack Recommandé (Mode Dégradé)\n" +
                "1. **Produit Hydratant de Base** : Nécessaire pour les types sensibles. \n" +
                "2. **Nettoyant Douceur** : Adapté à votre type de peau.\n" +
                "3. **Protection Solaire** : Indispensable pour toute routine.\n";
        }
    }
}
