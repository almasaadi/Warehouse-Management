using ManagmentSystem.Enums;
using ManagmentSystem.Models;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AD_project.Views
{
    // ==========================
    // Orders / Invoices Menu
    // ==========================
    public class OrderMenuView
    {
        public string ShowOrdersMenu()
        {
            return AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[bold blue]Orders / Invoices[/]")
                    .AddChoices(
                        "Show All Paid Orders",
                        "View Order Details",
                        "Back"));
        }

        public void ShowOrders(List<Order> orders)
        {
            if (!orders.Any())
            {
                AnsiConsole.MarkupLine("[yellow]No paid orders yet.[/]");
                return;
            }

            var table = new Table()
                .Border(TableBorder.Rounded)
                .Title("[bold cyan]Paid Orders[/]")
                .AddColumn("ID")
                .AddColumn("Date")
                .AddColumn("Items Count")
                .AddColumn("Total")
                .AddColumn("Status");

            foreach (var o in orders)
            {
                string statusColor = o.Status switch
                {
                    OrderStatus.Paid => "green",
                    OrderStatus.Cancelled => "red",
                    _ => "yellow"
                };

                table.AddRow(
                    o.Id.ToString(),
                    o.OrderDate.ToString("dd/MM HH:mm"),
                    o.Items.Count.ToString(),
                    o.Total.ToString("N2"),
                    $"[{statusColor}]{o.Status}[/]"
                );
            }

            AnsiConsole.Write(table);
        }

        public void ShowOrderDetails(Order order, List<Category> categories, List<Product> products)
        {
            AnsiConsole.MarkupLine($"[bold]Order #{order.Id} | {order.OrderDate:dd/MM HH:mm} | Status: [green]{order.Status}[/][/]");
            AnsiConsole.MarkupLine($"[bold green]Total: {order.Total:N2}[/]\n");

            var table = new Table()
                .AddColumn("Product")
                .AddColumn("Category")
                .AddColumn("Qty")
                .AddColumn("Price")
                .AddColumn("Subtotal");

            foreach (var item in order.Items)
            {
                var product = products.FirstOrDefault(p => p.Id == item.ProductId);
                var categoryName = product != null
                    ? categories.FirstOrDefault(c => c.Id == product.CategoryId)?.Name ?? "—"
                    : "—";

                table.AddRow(
                    item.ProductName,
                    categoryName,
                    item.Quantity.ToString(),
                    item.SalePrice.ToString("N2"),
                    item.SubTotal.ToString("N2")
                );
            }

            AnsiConsole.Write(table);
        }

        public int? AskOrderId(string prompt = "[blue]Enter Order ID (or 'exit'):[/]")
        {
            var input = AnsiConsole.Ask<string>(prompt);
            if (input.Trim().Equals("exit", StringComparison.OrdinalIgnoreCase))
                return null;

            return int.TryParse(input, out int id) ? id : (int?)null;
        }

        public bool ConfirmCancel(int orderId)
        {
            return AnsiConsole.Confirm($"Cancel order #{orderId}?", false);
        }

        public void ShowMessage(string message, bool success = true)
        {
            if (success)
                AnsiConsole.MarkupLine($"[green]✔ {message}[/]");
            else
                AnsiConsole.MarkupLine($"[red]✘ {message}[/]");
        }

        public void Wait()
        {
            AnsiConsole.MarkupLine("[grey]Press any key to continue...[/]");
            Console.ReadKey();
        }
    }
}
