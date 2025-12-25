namespace ProjetNet.Models.ViewModel
{
    public class CheckoutViewModel
    {
        public List<CartItem> CartItems { get; set; }
        public Order Order { get; set; }
        public decimal Total { get; set; }  // Ajoute cette ligne

    }
}
