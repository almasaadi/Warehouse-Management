using System;
using System.Linq;
using System.Collections.Generic;
using Spectre.Console;
using ManagmentSystem.Models;

namespace AD_project.Views
{
    public class POSMenuView
    {
        public string ShowMainMenu() => AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("[bold blue]POS Menu[/]")
                .AddChoices(
                    "Show Products",
                    "Add Product to Cart",
                    "Remove Product from Cart",
                    "View Cart",
                    "Checkout / Payment",
                    "Back"));

        public void ShowProducts(List<Product> products, List<Category> categories)
        {
            if (!products.Any())
            {
                AnsiConsole.MarkupLine("[yellow]No products available.[/]");
                return;
            }

            var table = new Table()
                .Border(TableBorder.Rounded)
                .Title("[bold green]Products[/]")
                .AddColumn("ID")
                .AddColumn("Name")
                .AddColumn("Category")
                .AddColumn("Qty")
                .AddColumn("Sale Price")
                .AddColumn("Description");

            foreach (var p in products)
            {
                var catName = categories.FirstOrDefault(c => c.Id == p.CategoryId)?.Name ?? "—";
                table.AddRow(p.Id.ToString(), p.Name, catName, p.Quantity.ToString(), p.SalePrice.ToString("N2"), string.IsNullOrWhiteSpace(p.Description) ? "—" : p.Description);
            }

            AnsiConsole.Write(table);
        }

        public void ShowCart(Cart cart, List<Category> categories)
        {
            if (!cart.Items.Any())
            {
                AnsiConsole.MarkupLine("[yellow]Cart is empty.[/]");
                return;
            }

            var table = new Table()
                .Border(TableBorder.Rounded)
                .Title("[bold magenta]Shopping Cart[/]")
                .AddColumn("Product ID")
                .AddColumn("Name")
                .AddColumn("Category")
                .AddColumn("Qty")
                .AddColumn("Price")
                .AddColumn("Subtotal");

            foreach (var item in cart.Items)
            {
                var categoryName = categories.FirstOrDefault(c => c.Id == item.ProductId)?.Name ?? "—";
                table.AddRow(item.ProductId.ToString(), item.ProductName, categoryName, item.Quantity.ToString(), item.SalePrice.ToString("N2"), item.Total.ToString("N2"));
            }

            AnsiConsole.MarkupLine($"[bold green]Total: {cart.Total:N2}[/]");
            AnsiConsole.Write(table);
        }

        public int AskProductId(string action) => AnsiConsole.Ask<int>($"Product ID to {action}:");
        public int AskQuantity(string action) => AnsiConsole.Ask<int>($"Quantity to {action}:");
        public decimal AskPaymentAmount(decimal total) => AnsiConsole.Ask<decimal>($"Total amount: {total:N2}. Enter received amount:");
        public bool Confirm(string message) => AnsiConsole.Confirm(message, false);

        public void ShowMessage(string message, bool success = true)
        {
            if (success) AnsiConsole.MarkupLine($"[green]✔ {message}[/]");
            else AnsiConsole.MarkupLine($"[red]✘ {message}[/]");
        }

        public void Wait() { AnsiConsole.MarkupLine("[grey]Press any key to continue...[/]"); Console.ReadKey(); }
    }
}
