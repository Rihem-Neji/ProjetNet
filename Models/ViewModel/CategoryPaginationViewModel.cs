using ProjetNet.Models;
using System.Collections.Generic;

namespace ProjetNet.Models.ViewModel
{
    public class CategoryPaginationViewModel
    {
        public Category Category { get; set; }
        public int Page { get; set; } 
        public int PageSize { get; set; } = 4; // 3 produits par page
        public int TotalPages { get; set; }      // nombre total de pages

        //public int TotalPages => (int)System.Math.Ceiling((double)Category.Products.Count / PageSize);
        public List<Product> Products { get; set; } // produits à afficher pour cette page

        public IEnumerable<Product> ProductsToShow =>
            Category.Products
                    .Skip((Page - 1) * PageSize)
                    .Take(PageSize);
    }
}
