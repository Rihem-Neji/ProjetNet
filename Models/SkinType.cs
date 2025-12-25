namespace ProjetNet.Models
{
    public class SkinType
    {
        public int Id { get; set; }
        public string Type { get; set; }

        public List<Product> Products { get; set; }
    }
}

