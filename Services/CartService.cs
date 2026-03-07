using System;
using System.Linq;
using ManagmentSystem.Models;
using ManagmentSystem.Contracts;

namespace ManagmentSystem.Services
{
    public class CartService : ICartService
    {
        private readonly IProductService _productService;
        private readonly OrderService _orderService;
        private Cart _cart;

        public CartService(IProductService productService, OrderService orderService)
        {
            _productService = productService;
            _orderService = orderService;
            _cart = new Cart();
        }

        public void AddToCart(Product product, int quantity)
        {
            if (product == null || product.IsDeleted)
                throw new Exception("Product not found or deleted.");
            if (quantity <= 0 || quantity > product.Quantity)
                throw new Exception("Invalid quantity.");

            var existing = _cart.Items.FirstOrDefault(i => i.ProductId == product.Id);
            if (existing != null) existing.Quantity += quantity;
            else
                _cart.Items.Add(new CartItem
                {
                    ProductId = product.Id,
                    ProductName = product.Name,
                    Quantity = quantity,
                    SalePrice = product.SalePrice
                });
        }

        public void UpdateItemQuantity(int productId, int newQuantity)
        {
            var item = _cart.Items.FirstOrDefault(i => i.ProductId == productId);
            if (item == null) throw new Exception("Product not in cart.");

            if (newQuantity <= 0) RemoveItem(productId);
            else item.Quantity = newQuantity;
        }

        public void RemoveItem(int productId)
        {
            var item = _cart.Items.FirstOrDefault(i => i.ProductId == productId);
            if (item != null) _cart.Items.Remove(item);
        }

        public void ClearCart() => _cart.Items.Clear();

        public Cart GetCart() => _cart;

        public Order PlaceOrder(string customerName)
        {
            if (!_cart.Items.Any())
                throw new Exception("Cart is empty.");

            if (string.IsNullOrWhiteSpace(customerName))
                throw new Exception("Customer name cannot be empty.");

            var order = new Order
            {
                CustomerName = customerName.Trim(),
                Items = _cart.Items.Select(i => new OrderItem
                {
                    ProductId = i.ProductId,
                    ProductName = i.ProductName,
                    Quantity = i.Quantity,
                    SalePrice = i.SalePrice
                }).ToList()
            };

            order.MarkAsPaid();              // POS: الدفع فورًا
            _orderService.SavePaidOrder(order); // حفظ الطلب في ملف منفصل
            ClearCart();                     // تفريغ السلة
            return order;
        }
    }
}
