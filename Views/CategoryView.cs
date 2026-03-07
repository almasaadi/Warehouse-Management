using Spectre.Console;
using System.Collections.Generic;
using System.Linq;
using ManagmentSystem.Models;

namespace ManagmentSystem.Views
{
    public class CategoryView
    {
        public string ShowMenu()
        {
            AnsiConsole.WriteLine();
            return AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[cyan]Category Management[/]")
                    .AddChoices("View Categories", "Add Category", "Edit Category", "Delete Category", "Back"));
        }

        public void ShowCategories(List<Category> categories)
        {
            AnsiConsole.Clear();
            AnsiConsole.Write(new Rule("[cyan]Category List[/]").RuleStyle("blue"));
            AnsiConsole.WriteLine();

            if (!categories.Any())
            {
                AnsiConsole.MarkupLine("[yellow]⚠ No categories available.[/]");
                return;
            }

            var table = new Table().Border(TableBorder.Rounded);
            table.AddColumn("[bold cyan]ID[/]");
            table.AddColumn("[bold cyan]Name[/]");

            foreach (var c in categories)
                table.AddRow(c.Id.ToString(), c.Name);

            AnsiConsole.Write(table);
        }

        public string AskCategoryName(string title = "Enter category name:") => AnsiConsole.Ask<string>($"[blue]{title}[/]");

        public int? AskCategoryId(string title = "Enter category ID:")
        {
            var input = AnsiConsole.Ask<string>($"[blue]{title}[/]");
            return int.TryParse(input, out int id) ? id : (int?)null;
        }

        public bool ConfirmDelete(string categoryName) => AnsiConsole.Confirm($"[red]Are you sure you want to delete '{categoryName}'?[/]", false);
    }
}