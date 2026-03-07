using System;
using System.Collections.Generic;
using System.Linq;

namespace ManagmentSystem.Models
{
    public class Cart
    {
        public List<CartItem> Items { get; private set; } = new List<CartItem>();

        public decimal Total => Items.Sum(i => i.Total);

        public void AddItem(int productId, string name, decimal price, int quantity)
        {
            var existing = Items.FirstOrDefault(i => i.ProductId == productId);
            if (existing != null)
                existing.Quantity += quantity;
            else
                Items.Add(new CartItem
                {
                    ProductId = productId,
                    ProductName = name,
                    SalePrice = price,
                    Quantity = quantity
                });
        }

        public void RemoveItem(int productId)
        {
            var item = Items.FirstOrDefault(i => i.ProductId == productId);
            if (item != null) Items.Remove(item);
        }

        public void Clear() => Items.Clear();
    }

  
}
