using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagmentSystem.Models
{
    public class OrderItem
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = "";
        public int Quantity { get; set; }
        public decimal SalePrice { get; set; }
        public decimal SubTotal => Quantity * SalePrice;
    }
}