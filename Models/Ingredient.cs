namespace ProjetNet.Models
{
    public class Ingredient
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public List<ProductIngredient> ProductIngredients { get; set; }
    }
}

