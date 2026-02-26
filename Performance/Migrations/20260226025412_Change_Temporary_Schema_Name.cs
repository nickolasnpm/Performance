using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Performance.Migrations
{
    /// <inheritdoc />
    public partial class Change_Temporary_Schema_Name : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Performance");

            migrationBuilder.RenameTable(
                name: "Users",
                schema: "PerformanceTemp",
                newName: "Users",
                newSchema: "Performance");

            migrationBuilder.RenameTable(
                name: "SupportTickets",
                schema: "PerformanceTemp",
                newName: "SupportTickets",
                newSchema: "Performance");

            migrationBuilder.RenameTable(
                name: "SupportTicketComments",
                schema: "PerformanceTemp",
                newName: "SupportTicketComments",
                newSchema: "Performance");

            migrationBuilder.RenameTable(
                name: "RoleUser",
                schema: "PerformanceTemp",
                newName: "RoleUser",
                newSchema: "Performance");

            migrationBuilder.RenameTable(
                name: "Roles",
                schema: "PerformanceTemp",
                newName: "Roles",
                newSchema: "Performance");

            migrationBuilder.RenameTable(
                name: "Loans",
                schema: "PerformanceTemp",
                newName: "Loans",
                newSchema: "Performance");

            migrationBuilder.RenameTable(
                name: "LoanRepayments",
                schema: "PerformanceTemp",
                newName: "LoanRepayments",
                newSchema: "Performance");

            migrationBuilder.RenameTable(
                name: "CreditCardStatements",
                schema: "PerformanceTemp",
                newName: "CreditCardStatements",
                newSchema: "Performance");

            migrationBuilder.RenameTable(
                name: "CreditCards",
                schema: "PerformanceTemp",
                newName: "CreditCards",
                newSchema: "Performance");

            migrationBuilder.RenameTable(
                name: "BankTransactions",
                schema: "PerformanceTemp",
                newName: "BankTransactions",
                newSchema: "Performance");

            migrationBuilder.RenameTable(
                name: "BankAccounts",
                schema: "PerformanceTemp",
                newName: "BankAccounts",
                newSchema: "Performance");

            migrationBuilder.RenameTable(
                name: "Addresses",
                schema: "PerformanceTemp",
                newName: "Addresses",
                newSchema: "Performance");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "PerformanceTemp");

            migrationBuilder.RenameTable(
                name: "Users",
                schema: "Performance",
                newName: "Users",
                newSchema: "PerformanceTemp");

            migrationBuilder.RenameTable(
                name: "SupportTickets",
                schema: "Performance",
                newName: "SupportTickets",
                newSchema: "PerformanceTemp");

            migrationBuilder.RenameTable(
                name: "SupportTicketComments",
                schema: "Performance",
                newName: "SupportTicketComments",
                newSchema: "PerformanceTemp");

            migrationBuilder.RenameTable(
                name: "RoleUser",
                schema: "Performance",
                newName: "RoleUser",
                newSchema: "PerformanceTemp");

            migrationBuilder.RenameTable(
                name: "Roles",
                schema: "Performance",
                newName: "Roles",
                newSchema: "PerformanceTemp");

            migrationBuilder.RenameTable(
                name: "Loans",
                schema: "Performance",
                newName: "Loans",
                newSchema: "PerformanceTemp");

            migrationBuilder.RenameTable(
                name: "LoanRepayments",
                schema: "Performance",
                newName: "LoanRepayments",
                newSchema: "PerformanceTemp");

            migrationBuilder.RenameTable(
                name: "CreditCardStatements",
                schema: "Performance",
                newName: "CreditCardStatements",
                newSchema: "PerformanceTemp");

            migrationBuilder.RenameTable(
                name: "CreditCards",
                schema: "Performance",
                newName: "CreditCards",
                newSchema: "PerformanceTemp");

            migrationBuilder.RenameTable(
                name: "BankTransactions",
                schema: "Performance",
                newName: "BankTransactions",
                newSchema: "PerformanceTemp");

            migrationBuilder.RenameTable(
                name: "BankAccounts",
                schema: "Performance",
                newName: "BankAccounts",
                newSchema: "PerformanceTemp");

            migrationBuilder.RenameTable(
                name: "Addresses",
                schema: "Performance",
                newName: "Addresses",
                newSchema: "PerformanceTemp");
        }
    }
}
