using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Performance.Migrations
{
    /// <inheritdoc />
    public partial class Drop_Legacy_Table_For_New_One : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                DROP TABLE IF EXISTS [Performance].[SupportTicketComments];
                DROP TABLE IF EXISTS [Performance].[SupportTickets];
                DROP TABLE IF EXISTS [Performance].[RoleUser];
                DROP TABLE IF EXISTS [Performance].[Roles];
                DROP TABLE IF EXISTS [Performance].[LoanRepayments];
                DROP TABLE IF EXISTS [Performance].[Loans];
                DROP TABLE IF EXISTS [Performance].[CreditCardStatements];
                DROP TABLE IF EXISTS [Performance].[CreditCards];
                DROP TABLE IF EXISTS [Performance].[BankTransactions];
                DROP TABLE IF EXISTS [Performance].[BankAccounts];
                DROP TABLE IF EXISTS [Performance].[Addresses];
                DROP TABLE IF EXISTS [Performance].[Users];

                DROP SCHEMA IF EXISTS [Performance];
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
