using ProjetNet.Models;

namespace ProjetNet.Models.ViewModel
{
    public class AIAgentResponseViewModel
    {
        public string AIRecommendation { get; set; }  // Texte généré par Gemini
        public List<Product> RecommendedProducts { get; set; }  // Produits recommandés
        public AIAgentRequestViewModel UserRequest { get; set; }  // Rappel de la demande
    }
}
