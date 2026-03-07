using ManagmentSystem.Models;
using System.Collections.Generic;
using System.Linq;

namespace ManagmentSystem.Extensions
{
    public static class InvoiceExtensions
    {
        public static bool IsInMonth(this Invoice invoice, int year, int month)
        {
            return invoice.InvoiceDate.Year == year &&
                   invoice.InvoiceDate.Month == month;
        }
        public static decimal CalculateProfit(this Invoice invoice, List<Product> products)
        {
            decimal totalProfit = 0;

            foreach (var item in invoice.Items)
            {
                var product = products.FirstOrDefault(p => p.Id == item.ProductId);

                if (product != null)
                {
                    totalProfit += (item.SalePrice - product.CostPrice) * item.Quantity;
                }
            }

            return totalProfit;
        }
    }
}