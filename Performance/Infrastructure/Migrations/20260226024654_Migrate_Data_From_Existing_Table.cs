using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Performance.Migrations
{
    /// <inheritdoc />
    public partial class Migrate_Data_From_Existing_Table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"

            SET IDENTITY_INSERT PerformanceTemp.Users ON;

            INSERT INTO PerformanceTemp.Users 
            (Id, Username, Email, FirstName, LastName, DateOfBirth, PhoneNumber, ProfilePictureUrl, IsEmailVerified, IsActive, LastLoginAt, CreatedAt, CreatedBy, UpdatedAt, UpdatedBy)
            SELECT 
            Id, Username, Email, FirstName, LastName, DateOfBirth, PhoneNumber, ProfilePictureUrl, IsEmailVerified, IsActive, LastLoginAt, CreatedAt, CreatedBy, UpdatedAt, UpdatedBy
            FROM Performance.Users

            SET IDENTITY_INSERT PerformanceTemp.Users OFF;


            SET IDENTITY_INSERT PerformanceTemp.Addresses ON;

            INSERT INTO PerformanceTemp.Addresses 
            (Id, AddressLine, City, State, PostalCode, Country, UserId, CreatedAt, CreatedBy, UpdatedAt, UpdatedBy)
            SELECT 
            Id, AddressLine, City, State, PostalCode, Country, UserId, CreatedAt, CreatedBy, UpdatedAt, UpdatedBy
            FROM Performance.Addresses

            SET IDENTITY_INSERT PerformanceTemp.Addresses OFF;


            SET IDENTITY_INSERT PerformanceTemp.BankAccounts ON;

            INSERT INTO PerformanceTemp.BankAccounts 
            (Id, AccountNumber, CurrentBalance, AvailableBalance, UserId, CreatedAt, CreatedBy, UpdatedAt, UpdatedBy)
            SELECT 
            Id, AccountNumber, CurrentBalance, AvailableBalance, UserId, CreatedAt, CreatedBy, UpdatedAt, UpdatedBy
            FROM Performance.BankAccounts

            SET IDENTITY_INSERT PerformanceTemp.BankAccounts OFF;


            SET IDENTITY_INSERT PerformanceTemp.BankTransactions ON;

            INSERT INTO PerformanceTemp.BankTransactions 
            (Id, BaseAmount, FeeAmount, SettlementAmount, TransactionType, Status, MerchantName, ReferenceNumber, BankAccountId, CreatedAt, CreatedBy, UpdatedAt, UpdatedBy)
            SELECT 
            Id, BaseAmount, FeeAmount, SettlementAmount, TransactionType, Status, MerchantName, ReferenceNumber, BankAccountId, CreatedAt, CreatedBy, UpdatedAt, UpdatedBy
            FROM Performance.BankTransactions

            SET IDENTITY_INSERT PerformanceTemp.BankTransactions OFF;

            
            SET IDENTITY_INSERT PerformanceTemp.CreditCards ON;

            INSERT INTO PerformanceTemp.CreditCards 
            (Id, CardNumber, CardHolderName, CardProvider, Bank, ExpiryMonth, ExpiryYear, IsDefault, CreditLimit, UserId, CreatedAt, CreatedBy, UpdatedAt, UpdatedBy)
            SELECT 
            Id, CardNumber, CardHolderName, CardProvider, Bank, ExpiryMonth, ExpiryYear, IsDefault, CreditLimit, UserId, CreatedAt, CreatedBy, UpdatedAt, UpdatedBy
            FROM Performance.CreditCards

            SET IDENTITY_INSERT PerformanceTemp.CreditCards OFF;

            
            SET IDENTITY_INSERT PerformanceTemp.CreditCardStatements ON;

            INSERT INTO PerformanceTemp.CreditCardStatements 
            (Id, StatementDate, DueDate, StatementBalance, MinimumPayment, PaymentsReceived, InterestCharged, AvailableCredit, CreditCardId, CreatedAt, CreatedBy, UpdatedAt, UpdatedBy)
            SELECT 
            Id, StatementDate, DueDate, StatementBalance, MinimumPayment, PaymentsReceived, InterestCharged, AvailableCredit, CreditCardId, CreatedAt, CreatedBy, UpdatedAt, UpdatedBy
            FROM Performance.CreditCardStatements

            SET IDENTITY_INSERT PerformanceTemp.CreditCardStatements OFF;


            SET IDENTITY_INSERT PerformanceTemp.Loans ON;

            INSERT INTO PerformanceTemp.Loans 
            (Id, LoanType, PrincipalAmount, InterestRate, InterestAmount, TotalAmountToRepay, RemainingBalance, TotalLoanTerms, RemainingLoanTerms, MonthlyPaymentAmount, IsFullyPaid, UserId, CreatedAt, CreatedBy, UpdatedAt, UpdatedBy)
            SELECT 
            Id, LoanType, PrincipalAmount, InterestRate, InterestAmount, TotalAmountToRepay, RemainingBalance, TotalLoanTerms, RemainingLoanTerms, MonthlyPaymentAmount, IsFullyPaid, UserId, CreatedAt, CreatedBy, UpdatedAt, UpdatedBy
            FROM Performance.Loans

            SET IDENTITY_INSERT PerformanceTemp.Loans OFF;


            SET IDENTITY_INSERT PerformanceTemp.LoanRepayments ON;

            INSERT INTO PerformanceTemp.LoanRepayments 
            (Id, ScheduledDate, ActualPaymentDate, ScheduledAmount, PaidAmount, LoanId, CreatedAt, CreatedBy, UpdatedAt, UpdatedBy)
            SELECT 
            Id, ScheduledDate, ActualPaymentDate, ScheduledAmount, PaidAmount, LoanId, CreatedAt, CreatedBy, UpdatedAt, UpdatedBy
            FROM Performance.LoanRepayments

            SET IDENTITY_INSERT PerformanceTemp.LoanRepayments OFF;

            
            SET IDENTITY_INSERT PerformanceTemp.Roles ON;

            INSERT INTO PerformanceTemp.Roles 
            (Id, Name, CreatedAt, CreatedBy, UpdatedAt, UpdatedBy)
            SELECT 
            Id, Name, CreatedAt, CreatedBy, UpdatedAt, UpdatedBy
            FROM Performance.Roles

            SET IDENTITY_INSERT PerformanceTemp.Roles OFF;


            INSERT INTO PerformanceTemp.RoleUser 
            (RolesId, UsersId)
            SELECT 
            RolesId, UsersId
            FROM Performance.RoleUser


            SET IDENTITY_INSERT PerformanceTemp.SupportTickets ON;

            INSERT INTO PerformanceTemp.SupportTickets
            (Id, Subject, Description, Priority, IsResolved, UserId, CreatedAt, CreatedBy, UpdatedAt, UpdatedBy)
            SELECT 
            Id, Subject, Description, Priority, IsResolved, UserId, CreatedAt, CreatedBy, UpdatedAt, UpdatedBy
            FROM Performance.SupportTickets

            SET IDENTITY_INSERT PerformanceTemp.SupportTickets OFF;


            SET IDENTITY_INSERT PerformanceTemp.SupportTicketComments ON;

            INSERT INTO PerformanceTemp.SupportTicketComments
            (Id, CommentText, RecommendedAction, TicketId, CreatedAt, CreatedBy, UpdatedAt, UpdatedBy)
            SELECT 
            Id, CommentText, RecommendedAction, TicketId, CreatedAt, CreatedBy, UpdatedAt, UpdatedBy
            FROM Performance.SupportTicketComments

            SET IDENTITY_INSERT PerformanceTemp.SupportTicketComments OFF;

            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
