namespace ManagmentSystem.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal SalePrice { get; set; }   
        public decimal CostPrice { get; set; }   
        public int CategoryId { get; set; }
        public bool IsDeleted { get; set; } = false; 
    }
}
