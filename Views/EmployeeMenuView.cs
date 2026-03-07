using Spectre.Console;
using System.Collections.Generic;
using ManagmentSystem.Models;
using ManagmentSystem.Enums;

namespace ManagmentSystem.Views
{
    public class EmployeeMenuView
    {
        public string ShowMainMenu()
        {
            AnsiConsole.WriteLine();
            return AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[blue]Select an action:[/]")
                    .PageSize(10)
                    .AddChoices("View All", "Add", "Update", "Delete", "Back"));
        }

        public void DisplayEmployeesTable(List<Employee> employees)
        {
            AnsiConsole.Clear();
            AnsiConsole.Write(new Rule("[yellow]Employee Management[/]").RuleStyle("blue"));
            AnsiConsole.WriteLine();

            var table = new Table().Border(TableBorder.Rounded).Expand();
            table.AddColumn("[bold yellow]Username[/]");
            table.AddColumn("[bold yellow]Full Name[/]");
            table.AddColumn("[bold yellow]Role[/]");

            foreach (var e in employees)
            {
                table.AddRow(e.Username, e.PersonalInfo.FullName, e.Role.ToString());
            }

            AnsiConsole.Write(table);
        }

        public string GetUsernameToDelete() => AnsiConsole.Ask<string>("[red]Enter username to delete:[/]");
        public string GetUsernameToUpdate() => AnsiConsole.Ask<string>("[blue]Enter username to update:[/]");

        public (string user, string pass, string fname, string lname, string phone, UserRole role) GetNewEmployeeDetails()
        {
            AnsiConsole.MarkupLine("[yellow]Enter New Employee Details:[/]");
            return (
                AnsiConsole.Ask<string>("Username:"),
                AnsiConsole.Ask<string>("Password:"),
                AnsiConsole.Ask<string>("First Name:"),
                AnsiConsole.Ask<string>("Last Name:"),
                AnsiConsole.Ask<string>("Phone Number:"),
                AnsiConsole.Prompt(new SelectionPrompt<UserRole>().Title("Select Role:").AddChoices(UserRole.Manager, UserRole.Employee))
            );
        }
    }
}