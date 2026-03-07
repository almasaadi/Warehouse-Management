using ManagmentSystem.Models;

namespace ManagmentSystem.Contracts
{
    public interface ICartService
    {
        void AddToCart(Product product, int quantity);
        void UpdateItemQuantity(int productId, int newQuantity);
        void RemoveItem(int productId);
        void ClearCart();
        Cart GetCart();
        Order PlaceOrder(string customerName = "Guest");
    }
}
