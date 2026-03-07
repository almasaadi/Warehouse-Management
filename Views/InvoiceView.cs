using Spectre.Console;
using ManagmentSystem.Models;
using ManagmentSystem.Helpers;

namespace ManagmentSystem.Views

{
    public class InvoiceView
    {
        // 1. عرض فاتورة واحدة بشكل مفصل (للإصدار أو العرض المفرد)
        public void DisplayInvoiceDetails(Invoice invoice)
        {
            AnsiConsole.Clear();

            // استخدام الـ Panel لعرض نص الفورماتر داخل إطار ملون
            var panel = new Panel(InvoiceFormatter.Format(invoice))
            {
                Header = new PanelHeader($"[bold cyan]Invoice #{invoice.Id}[/]"),
                Border = BoxBorder.Rounded,
                Padding = new Padding(2, 1, 2, 1),
                Expand = true
            };

            AnsiConsole.Write(panel);
        }

        // 2. عرض قائمة الفواتير (نتائج البحث) باستخدام Table من Spectre.Console
        public void DisplayInvoicesList(List<Invoice> invoices, string searchCriteria)
        {
            AnsiConsole.Clear();
            AnsiConsole.Write(new Rule($"[yellow]Search Results for: {searchCriteria}[/]").Centered());
            AnsiConsole.WriteLine();

            if (invoices == null || !invoices.Any())
            {
                AnsiConsole.MarkupLine("[red]No invoices found matching your criteria.[/]");
                return;
            }

            // إنشاء جدول احترافي
            var table = new Table();
            table.Border(TableBorder.Rounded);
            table.AddColumn("[blue]ID[/]");
            table.AddColumn("[blue]Date[/]");
            table.AddColumn("[blue]Customer[/]");
            table.AddColumn("[blue]Employee[/]");
            table.AddColumn("[green]Total[/]");

            foreach (var inv in invoices)
            {
                table.AddRow(
                    inv.Id.ToString(),
                    inv.InvoiceDate.ToShortDateString(),
                    inv.CustomerName,
                    inv.EmployeeName, // هذا هو الاسم الكامل الذي سحبناه من الجلسة
                    $"[bold green]{inv.Total:N0}[/]"
                );
            }

            AnsiConsole.Write(table);
            AnsiConsole.MarkupLine($"[grey]Found {invoices.Count} invoice(s).[/]");
        }

        // 3. طلب اسم العميل للبحث
        public string PromptCustomerSearch()
        {
            return AnsiConsole.Ask<string>("Please enter [green]Customer Name[/] to search:");
        }

        // 4. عرض رسائل الخطأ
        public void ShowError(string message)
        {
            AnsiConsole.Write(new Panel($"[red]{message}[/]") { Border = BoxBorder.None });
        }

        // 5. رسالة نجاح
        public void ShowSuccess(string message)
        {
            AnsiConsole.MarkupLine($"[bold green]✔ {message}[/]");
        }
    }
}