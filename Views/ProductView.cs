using System;
using System.Collections.Generic;
using System.Linq;
using ManagmentSystem.Models;
using Spectre.Console;

namespace ManagmentSystem.Views
{
    public class ProductView
    {
        public void ShowProducts(List<Product> products, List<Category> categories)
        {
            if (!products.Any())
            {
                AnsiConsole.MarkupLine("[yellow]⚠️ No products available.[/]");
                return;
            }

            var table = new Table()
                .Border(TableBorder.Rounded)
                .Title("[bold green]Inventory List[/]")
                .AddColumn("[green]ID[/]")
                .AddColumn("[blue]Name[/]")
                .AddColumn("[grey]Category[/]")
                .AddColumn("[yellow]Qty[/]")
                .AddColumn("[red]Sale Price[/]")
                .AddColumn("[maroon]Cost Price[/]") // تمت إضافته ليطابق الموديل
                .AddColumn("[dim]Description[/]");

            foreach (var p in products)
            {
                var catName = categories.FirstOrDefault(c => c.Id == p.CategoryId)?.Name ?? "—";
                var desc = string.IsNullOrWhiteSpace(p.Description) ? "—" :
                    (p.Description.Length > 30 ? p.Description.Substring(0, 30) + "..." : p.Description);

                table.AddRow(
                    p.Id.ToString(),
                    p.Name,
                    catName,
                    p.Quantity.ToString(),
                    p.SalePrice.ToString("N2") + " SAR",
                    p.CostPrice.ToString("N2") + " SAR",
                    desc
                );
            }
            AnsiConsole.Write(table);
        }

        // تم تعديل هذه الدالة لترجع 6 قيم بدلاً من 5 لتطابق الـ Service
        public (string name, string desc, int qty, decimal salePrice, decimal costPrice, int catId)? AskProductDetails(List<Category> categories)
        {
            if (!categories.Any())
            {
                AnsiConsole.MarkupLine("[red]❌ Cannot add product: No categories found. Add a category first.[/]");
                return null;
            }

            // عرض التصنيفات للمستخدم ليختار منها
            var catTable = new Table().Border(TableBorder.Minimal).Title("Available Categories");
            catTable.AddColumn("ID"); catTable.AddColumn("Name");
            foreach (var c in categories) catTable.AddRow(c.Id.ToString(), c.Name);
            AnsiConsole.Write(catTable);

            try
            {
                var name = AnsiConsole.Ask<string>("[blue]Product Name:[/]");
                var desc = AnsiConsole.Prompt(new TextPrompt<string>("[blue]Description (Optional):[/]").AllowEmpty());
                var qty = AnsiConsole.Ask<int>("[blue]Quantity:[/]");
                var salePrice = AnsiConsole.Ask<decimal>("[blue]Sale Price (Selling):[/]");
                var costPrice = AnsiConsole.Ask<decimal>("[blue]Cost Price (Purchase):[/]"); // القيمة المفقودة
                var catId = AnsiConsole.Ask<int>("[blue]Category ID:[/]");

                return (name, desc, qty, salePrice, costPrice, catId);
            }
            catch
            {
                AnsiConsole.MarkupLine("[red]Invalid input format.[/]");
                return null;
            }
        }

        public string ShowProductsMenu()
        {
            return AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[green]📦 Product Management[/]")
                    .AddChoices("Show All", "Add Product", "Edit Product", "Delete Product", "Search", "Back")
            );
        }

        public int AskProductId(string action) => AnsiConsole.Ask<int>($"[yellow]Enter Product ID to {action}:[/]");
    }
}