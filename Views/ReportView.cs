using System;
using System.Collections.Generic;
using System.Linq;
using Spectre.Console;
using ManagmentSystem.Services;
using ManagmentSystem.Helpers;
using ManagmentSystem.Exceptions;
using ManagmentSystem.Models;

namespace ManagmentSystem.Views
{
    public class ReportView
    {
        // الدالة الرئيسية لعرض القائمة
        public string ShowReportsMenu()
        {
            AnsiConsole.Clear();
            AnsiConsole.Write(new Rule("[yellow]📊 Reports Management[/]").RuleStyle("blue"));
            AnsiConsole.WriteLine();

            return AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Select Report Type:")
                    .AddChoices("Monthly Sales & Profit Report", "Back"));
        }

        public void ShowMonthlyReport(InvoiceService invoiceService, ProductService productService)
        {
            AnsiConsole.Clear();
            AnsiConsole.Write(new Rule("[green]Monthly Profit Analysis[/]").RuleStyle("white"));

            int year = AnsiConsole.Ask<int>("[white]Enter Year (e.g. 2025):[/]");
            int month = AnsiConsole.Ask<int>("[white]Enter Month (1-12):[/]");

            if (month < 1 || month > 12)
            {
                AnsiConsole.MarkupLine("[red]Invalid month.[/]");
                Console.ReadKey(true);
                return;
            }

            try
            {
                var invoices = invoiceService.GetMonthlyInvoices(year, month);
                var products = productService.GetAllProducts();

                // بناء نص التقرير باستخدام الـ Helper الخاص بكِ
                var reportText = ReportBuilder.BuildMonthlySalesReport(invoices, products, year, month);

                // حساب الأرقام المالية (الربح)
                decimal totalSales = invoices.Sum(i => i.Total);
                decimal totalCost = 0;

                foreach (var inv in invoices)
                {
                    foreach (var item in inv.Items)
                    {
                        var prod = products.FirstOrDefault(p => p.Id == item.ProductId);
                        totalCost += (prod?.CostPrice ?? 0) * item.Quantity;
                    }
                }

                decimal netProfit = totalSales - totalCost;

                // عرض النتائج في لوحة
                var panel = new Panel(reportText +
                    $"\n[bold green]Total Revenue:[/] {totalSales:N2} SAR" +
                    $"\n[bold blue]Total Costs:[/] {totalCost:N2} SAR" +
                    $"\n[bold gold1]Net Profit:[/] {netProfit:N2} SAR")
                {
                    Header = new PanelHeader($" Monthly Report {month}/{year} "),
                    Border = BoxBorder.Double,
                    Expand = true
                };

                AnsiConsole.Write(panel);
            }
            catch (InvoiceNotFoundException ex)
            {
                AnsiConsole.MarkupLine($"[yellow]⚠ {ex.Message}[/]");
            }
            catch (Exception ex)
            {
                AnsiConsole.MarkupLine($"[red]❌ Error: {ex.Message}[/]");
            }

            AnsiConsole.MarkupLine("\n[grey]Press any key to return...[/]");
            Console.ReadKey(true);
        }
    }
}