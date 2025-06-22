namespace ShoppingCart.Domain.Models
{
    public class Cart
    {
        public int Id { get; set; }
        public int UserId { get; set; } // Użyjemy UserId do identyfikacji właściciela koszyka
        public List<CartItem> Items { get; set; } = new List<CartItem>();
    }
}
