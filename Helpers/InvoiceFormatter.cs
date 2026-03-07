using System.Text;
using ManagmentSystem.Models;

namespace ManagmentSystem.Helpers
{
    public static class InvoiceFormatter
    {
        public static string Format(Invoice invoice)
        {
            var sb = new StringBuilder();

            sb.AppendLine("=================================");
            sb.AppendLine($"INVOICE #{invoice.Id}");
            sb.AppendLine($"Date     : {invoice.InvoiceDate}");
            sb.AppendLine($"Customer : {invoice.CustomerName}");
            sb.AppendLine("---------------------------------");
            sb.AppendLine($"Issued By: {invoice.EmployeeName}");

            foreach (var item in invoice.Items)
            {
                sb.AppendLine(
                    $"{item.ProductName,-15} x{item.Quantity,-3} = {item.SubTotal,8:N0}");
            }

            sb.AppendLine("---------------------------------");
            sb.AppendLine($"TOTAL: {invoice.Total:N0}");
            sb.AppendLine("=================================");

            return sb.ToString();
        }
        public static string InvoicesListFormat(List<Invoice> invoices)
        {
            var sb = new StringBuilder();
            sb.AppendLine("ID | Date | Customer | Total");
            sb.AppendLine("--------------------------------");

            foreach (var inv in invoices)
            {
                sb.AppendLine(
                    $"{inv.Id} | {inv.InvoiceDate:d} | {inv.CustomerName} | {inv.Total:N0}");
            }

            return sb.ToString();
        }
    }
}





























