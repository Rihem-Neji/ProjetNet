namespace ProjetNet.Models
{
    public class Product

    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string ImageUrl { get; set; }

        public int CategoryId { get; set; }
        public Category Category { get; set; }

        public int BrandId { get; set; }
        public Brand Brand { get; set; }

        public int SkinTypeId { get; set; }
        public SkinType SkinType { get; set; }

        public List<ProductIngredient> ProductIngredients { get; set; }
        public List<Review> Reviews { get; set; }
    }
}

