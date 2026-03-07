using Spectre.Console;
using System;

namespace ManagmentSystem.Views
{
    public static class MessageView
    {
        public static void ShowError(string message)
        {
            AnsiConsole.MarkupLine($"[bold red]✘ Error:[/] [red]{message}[/]");
            Wait();
        }

        public static void ShowSuccess(string message)
        {
            AnsiConsole.MarkupLine($"[bold green]✔ Success:[/] [green]{message}[/]");
            Wait();
        }

        public static void Wait()
        {
            AnsiConsole.WriteLine();
            AnsiConsole.MarkupLine("[grey]Press any key to continue...[/]");
            Console.ReadKey(true);
        }
    }
}