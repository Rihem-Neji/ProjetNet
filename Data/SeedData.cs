using ProjetNet.Models;
using System.Linq;

namespace ProjetNet.Data
{
    public static class SeedData
    {
        public static void Initialize(AppDbContext context)
        {
            if (context.Products.Any())
                return;

            // ------------------ BRANDS ------------------
            var brandLaRoche = new Brand { Name = "La Roche-Posay" };
            var brandCeraVe = new Brand { Name = "CeraVe" };
            var brandSVR = new Brand { Name = "SVR" };
            var brandVichy = new Brand { Name = "Vichy" };
            var brandOrdinary = new Brand { Name = "The Ordinary" };
            var brandEucerin = new Brand { Name = "Eucerin" };

            context.Brands.AddRange(brandLaRoche, brandCeraVe, brandSVR, brandVichy, brandOrdinary, brandEucerin);

            // ------------------ CATEGORIES ------------------
            var catCleanser = new Category { Name = "Cleanser" };
            var catSerum = new Category { Name = "Serum" };
            var catMoisturizer = new Category { Name = "Moisturizer" };
            var catSunscreen = new Category { Name = "Sunscreen" };
            var catMask = new Category { Name = "Face Mask" };
            var catAccessory = new Category { Name = "Accessory" };

            context.Categories.AddRange(catCleanser, catSerum, catMoisturizer, catSunscreen, catMask, catAccessory);

            // ------------------ SKINTYPES ------------------
            var skinDry = new SkinType { Type = "Dry" };
            var skinOily = new SkinType { Type = "Oily" };
            var skinCombo = new SkinType { Type = "Combination" };
            var skinSensitive = new SkinType { Type = "Sensitive" };

            context.SkinTypes.AddRange(skinDry, skinOily, skinCombo, skinSensitive);

            // ------------------ INGREDIENTS ------------------
            var ingHyaluronic = new Ingredient { Name = "Hyaluronic Acid" };
            var ingNiacinamide = new Ingredient { Name = "Niacinamide" };
            var ingVitaminC = new Ingredient { Name = "Vitamin C" };
            var ingRetinol = new Ingredient { Name = "Retinol" };
            var ingZinc = new Ingredient { Name = "Zinc" };
            var ingCeramides = new Ingredient { Name = "Ceramides" };
            var ingUrea = new Ingredient { Name = "Urea" };
            var ingSPF = new Ingredient { Name = "SPF 50" };
            var ingAHA = new Ingredient { Name = "AHA" };
            var ingBHA = new Ingredient { Name = "BHA" };

            context.Ingredients.AddRange(
                ingHyaluronic, ingNiacinamide, ingVitaminC, ingRetinol,
                ingZinc, ingCeramides, ingUrea, ingSPF, ingAHA, ingBHA
            );

            // ------------------ PRODUCTS ------------------
            var products = new[]
{
    // Cleanser
    new Product { Name="La Roche-Posay Effaclar Purifying Foaming Gel", Brand=brandLaRoche, Category=catCleanser, SkinType=skinOily, Price=15.99M, Description="Foaming cleanser for oily skin", ImageUrl="foaming_cleanser.png" },
    new Product { Name="CeraVe Hydrating Facial Cleanser", Brand=brandCeraVe, Category=catCleanser, SkinType=skinDry, Price=13.50M, Description="Gentle hydrating cleanser", ImageUrl="cerave-hydrating-cleanser.jpg" },
    new Product { Name="SVR Physiopure Foaming Gel", Brand=brandSVR, Category=catCleanser, SkinType=skinSensitive, Price=16.50M, Description="Gentle cleanser for sensitive skin", ImageUrl="SVRPhysi1.png" },
    new Product { Name="Eucerin DermatoCLEAN Cleansing Gel", Brand=brandEucerin, Category=catCleanser, SkinType=skinCombo, Price=14.00M, Description="Dermatological cleanser for combination skin", ImageUrl="Eucerin DermatoCL2.png" },

    // Serum
    new Product { Name="The Ordinary Niacinamide 10% + Zinc 1%", Brand=brandOrdinary, Category=catSerum, SkinType=skinOily, Price=11.99M, Description="Brightening serum for oily skin", ImageUrl="ordinary-niacinamide.jpg" },
    new Product { Name="The Ordinary Retinol in Squalane", Brand=brandOrdinary, Category=catSerum, SkinType=skinSensitive, Price=13.99M, Description="Retinol serum for sensitive skin", ImageUrl="ordinary-retinol.jpg" },
    new Product { Name="La Roche-Posay Pure Vitamin C10", Brand=brandLaRoche, Category=catSerum, SkinType=skinDry, Price=19.99M, Description="Vitamin C serum for radiant skin", ImageUrl="la-roche-vitamin-c10.jpg" },
    new Product { Name="Vichy Mineral 89 Hyaluronic Acid Booster", Brand=brandVichy, Category=catSerum, SkinType=skinCombo, Price=21.50M, Description="Hydrating booster with hyaluronic acid", ImageUrl="vichy-mineral-89.jpg" },
    new Product { Name="SVR Ampoule A Retinol", Brand=brandSVR, Category=catSerum, SkinType=skinSensitive, Price=18.00M, Description="Retinol serum ampoules", ImageUrl="hydrating_serum.png" },
    new Product { Name="CeraVe Vitamin C Serum 10%", Brand=brandCeraVe, Category=catSerum, SkinType=skinDry, Price=15.99M, Description="Brightening serum with Vitamin C", ImageUrl="cerave-vitamin-c.jpg" },

    // Moisturizer
    new Product { Name="CeraVe Moisturizing Cream", Brand=brandCeraVe, Category=catMoisturizer, SkinType=skinDry, Price=12.99M, Description="Hydrating cream for dry skin", ImageUrl="CeraVe Moistu3.png" },
    new Product { Name="La Roche-Posay Toleriane Sensitive Creme", Brand=brandLaRoche, Category=catMoisturizer, SkinType=skinSensitive, Price=18.50M, Description="Calming cream for sensitive skin", ImageUrl="-PosayTolerianeSe4.png" },
    new Product { Name="Vichy Liftactiv Collagen Specialist", Brand=brandVichy, Category=catMoisturizer, SkinType=skinCombo, Price=25.00M, Description="Anti-aging cream for combo skin", ImageUrl="VichyLiftactivCollagen.jpg" },
    new Product { Name="SVR Topialyse Creme", Brand=brandSVR, Category=catMoisturizer, SkinType=skinDry, Price=16.50M, Description="Hydrating body cream", ImageUrl="SVRTopial5.png" },
    new Product { Name="Eucerin UreaRepair Plus 10% Urea", Brand=brandEucerin, Category=catMoisturizer, SkinType=skinDry, Price=14.50M, Description="Repairing cream for very dry skin", ImageUrl="EucerinUreaRe7.png" },

    // Sunscreen
    new Product { Name="La Roche-Posay Anthelios UVMune 400 SPF50", Brand=brandLaRoche, Category=catSunscreen, SkinType=skinSensitive, Price=20.00M, Description="High protection sunscreen", ImageUrl="Roche-PosayAnthelios77.png" },
    new Product { Name="CeraVe Facial Moisturizing Lotion AM SPF 50", Brand=brandCeraVe, Category=catSunscreen, SkinType=skinDry, Price=19.50M, Description="Moisturizing sunscreen SPF50", ImageUrl="cerave-spf50.jpg" },
    new Product { Name="SVR Sun Secure SPF50+ Fluid", Brand=brandSVR, Category=catSunscreen, SkinType=skinSensitive, Price=21.00M, Description="Sun protection fluid", ImageUrl="SVRSunSecure8.png" },
    new Product { Name="Eucerin Sun Oil Control SPF50+", Brand=brandEucerin, Category=catSunscreen, SkinType=skinOily, Price=18.00M, Description="Oil control sunscreen SPF50", ImageUrl="EucerinSunOil9.png" },

    // Face Mask
    new Product { Name="The Ordinary AHA 30% BHA 2% Peeling Solution", Brand=brandOrdinary, Category=catMask, SkinType=skinOily, Price=12.50M, Description="Chemical exfoliating mask", ImageUrl="ordinary-aha-bha.jpg" },
    new Product { Name="Vichy Double Glow Peel Mask", Brand=brandVichy, Category=catMask, SkinType=skinCombo, Price=14.99M, Description="Brightening peel mask", ImageUrl="Vichy10Double.png" },
    new Product { Name="SVR Sebiaclear Mask", Brand=brandSVR, Category=catMask, SkinType=skinOily, Price=16.00M, Description="Purifying mask", ImageUrl="svr-sebiaclear-mask.jpg" },

    // Accessories
    new Product { Name="Jade Roller", Brand=brandOrdinary, Category=catAccessory, SkinType=skinSensitive, Price=10.00M, Description="Facial massage tool", ImageUrl="jade-roller.jpg" },
    new Product { Name="Gua Sha", Brand=brandVichy, Category=catAccessory, SkinType=skinSensitive, Price=12.50M, Description="Facial massage tool", ImageUrl="GuaSha11.png" },

    
};

            context.Products.AddRange(products);

            // ------------------ PRODUCT INGREDIENTS ------------------
            context.ProductIngredients.AddRange(
                new ProductIngredient { Product = products[0], Ingredient = ingZinc },
                new ProductIngredient { Product = products[0], Ingredient = ingNiacinamide },
                new ProductIngredient { Product = products[1], Ingredient = ingCeramides },
                new ProductIngredient { Product = products[2], Ingredient = ingCeramides },
                new ProductIngredient { Product = products[3], Ingredient = ingCeramides },
                new ProductIngredient { Product = products[4], Ingredient = ingNiacinamide },
                new ProductIngredient { Product = products[4], Ingredient = ingZinc },
                new ProductIngredient { Product = products[5], Ingredient = ingRetinol },
                new ProductIngredient { Product = products[6], Ingredient = ingVitaminC },
                new ProductIngredient { Product = products[7], Ingredient = ingHyaluronic },
                new ProductIngredient { Product = products[8], Ingredient = ingRetinol },
                new ProductIngredient { Product = products[9], Ingredient = ingVitaminC },
                new ProductIngredient { Product = products[10], Ingredient = ingCeramides },
                new ProductIngredient { Product = products[11], Ingredient = ingVitaminC },
                new ProductIngredient { Product = products[12], Ingredient = ingHyaluronic },
                new ProductIngredient { Product = products[13], Ingredient = ingCeramides },
                new ProductIngredient { Product = products[14], Ingredient = ingUrea },
                new ProductIngredient { Product = products[15], Ingredient = ingSPF },
                new ProductIngredient { Product = products[16], Ingredient = ingSPF },
                new ProductIngredient { Product = products[17], Ingredient = ingSPF },
                new ProductIngredient { Product = products[18], Ingredient = ingSPF },
                new ProductIngredient { Product = products[19], Ingredient = ingAHA },
                new ProductIngredient { Product = products[20], Ingredient = ingAHA },
                new ProductIngredient { Product = products[21], Ingredient = ingBHA }
            );

            context.SaveChanges();
        }
    }
}
