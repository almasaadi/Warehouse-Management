namespace ManagmentSystem.Models
{
    public class CartItem
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal SalePrice { get; set; }

        public decimal Total => Quantity * SalePrice;
    }
}
