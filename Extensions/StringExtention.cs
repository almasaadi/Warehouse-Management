using ManagmentSystem.Models;

namespace ManagmentSystem.Extensions
{
    public static class StringExtention
    {
        public static bool IsValidProductName(this string name)
            => !string.IsNullOrWhiteSpace(name) && name.Length <= 100;

        public static bool IsValidDescription(this string desc)
            => !string.IsNullOrWhiteSpace(desc) && desc.Length <= 500;
    }

    public static class ProductExtensions
    {
        public static string ToDisplayLine(this Product product)
        {
            return $"[green]{product.Name}[/] | Qty: {product.Quantity} | SAR {product.SalePrice:N2}";
        }
    }
}
