using System.Text;
using ManagmentSystem.Extensions;
using ManagmentSystem.Models;

namespace ManagmentSystem.Helpers
{
    public static class ReportBuilder
    {
        public static string BuildMonthlySalesReport(
            List<Invoice> invoices,
            List<Product> products,
            int year,
            int month)
        {
            var sb = new StringBuilder();
            decimal totalProfit = 0;

            sb.AppendLine($"Monthly Profit Report - {month}/{year}");
            sb.AppendLine("==============================================");
            sb.AppendLine("Date       | Customer        | Profit");
            sb.AppendLine("----------------------------------------------");

            foreach (var invoice in invoices)
            {
                decimal invoiceProfit = invoice.CalculateProfit(products);

                sb.AppendLine(
                    $"{invoice.InvoiceDate:yyyy-MM-dd} | " +
                    $"{invoice.CustomerName,-15} | " +
                    $"{invoiceProfit,8:N0}");

                totalProfit += invoiceProfit;
            }

            sb.AppendLine("----------------------------------------------");
            sb.AppendLine($"TOTAL PROFIT: {totalProfit:N0}");
            sb.AppendLine("==============================================");

            return sb.ToString();
        }
    }
}
