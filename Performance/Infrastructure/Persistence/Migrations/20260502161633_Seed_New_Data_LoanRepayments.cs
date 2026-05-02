using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Performance.Migrations
{
    /// <inheritdoc />
    public partial class Seed_New_Data_LoanRepayments : Migration
    {
        private const int totalData = 1000000;
        private const int startingNumber = 110000;
        private readonly int endingNumber = totalData + startingNumber;

        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql($@"
                DECLARE @i      INT = {startingNumber};
                DECLARE @loanId BIGINT;

                WHILE @i < {endingNumber}
                BEGIN
                    SELECT @loanId = [Id]
                    FROM [Performance].[Loans]
                    WHERE [UserId] = (
                        SELECT [Id]
                        FROM [Performance].[Users]
                        WHERE [Username] = N'user' + CAST(@i AS NVARCHAR(10))
                    );

                    INSERT INTO [Performance].[LoanRepayments]
                        ([ScheduledDate],
                         [ActualPaymentDate],
                         [ScheduledAmount],
                         [PaidAmount],
                         [LoanId],
                         [CreatedAt],
                         [CreatedBy],
                         [UpdatedAt],
                         [UpdatedBy])
                    VALUES
                        (
                            -- ScheduledDate: 1 month from today
                            CAST(DATEADD(MONTH, 1, GETUTCDATE()) AS DATE),

                            -- ActualPaymentDate: null (not paid yet)
                            NULL,

                            -- ScheduledAmount: same as MonthlyPaymentAmount in Loans
                            CAST(2387.08 AS DECIMAL(18, 2)),

                            -- PaidAmount: 0 (not paid yet)
                            CAST(0 AS DECIMAL(18, 2)),

                            @loanId,

                            SYSDATETIMEOFFSET(),
                            N'data seeding',
                            SYSDATETIMEOFFSET(),
                            N'data seeding'
                        );

                    SET @i = @i + 1;
                END
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                DELETE FROM [Performance].[LoanRepayments]
                WHERE [CreatedBy] = 'data seeding';
            ");
        }
    }
}