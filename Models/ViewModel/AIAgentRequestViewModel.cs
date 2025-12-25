using System.ComponentModel.DataAnnotations;
namespace ProjetNet.Models.ViewModel
{
    public class AIAgentRequestViewModel
    {
        public int SkinTypeId { get; set; }
        public string Allergies { get; set; }
        public string AdditionalConcerns { get; set; }  // Renommé pour correspondre à la vue
    }
}
