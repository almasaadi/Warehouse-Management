using Spectre.Console;
using System.Threading;

namespace ManagmentSystem.Views
{
    public class LoginView
    {
        public (string username, string password) ShowLoginScreen()
        {
            AnsiConsole.Clear();
            AnsiConsole.Write(new Rule("[yellow]Warehouse System[/]").RuleStyle("blue"));
            AnsiConsole.WriteLine();

            var username = AnsiConsole.Ask<string>("[white]Enter Username:[/]");
            var password = AnsiConsole.Prompt(
                new TextPrompt<string>("[white]Enter Password:[/]")
                    .PromptStyle("red")
                    .Secret()
            );

            return (username, password);
        }

        public void ShowError(string message)
        {
            AnsiConsole.MarkupLine($"[red]✘ {message}[/]");
            Thread.Sleep(2000);
        }

        public void ShowSuccess(string message)
        {
            AnsiConsole.MarkupLine($"[green]✔ {message}[/]");
            Thread.Sleep(1500);
        }
    }
}