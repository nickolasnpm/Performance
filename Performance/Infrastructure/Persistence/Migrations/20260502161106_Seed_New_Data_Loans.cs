using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Performance.Migrations
{
    /// <inheritdoc />
    public partial class Seed_New_Data_Loans : Migration
    {
        private const int totalData = 1000000;
        private const int startingNumber = 110000;
        private readonly int endingNumber = totalData + startingNumber;

        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql($@"
                DECLARE @i      INT = {startingNumber};
                DECLARE @userId BIGINT;

                WHILE @i < {endingNumber}
                BEGIN
                    SELECT @userId = [Id]
                    FROM [Performance].[Users]
                    WHERE [Username] = N'user' + CAST(@i AS NVARCHAR(10));

                    INSERT INTO [Performance].[Loans]
                        ([LoanType],
                         [PrincipalAmount],
                         [InterestRate],
                         [InterestAmount],
                         [TotalAmountToRepay],
                         [RemainingBalance],
                         [TotalLoanTerms],
                         [RemainingLoanTerms],
                         [MonthlyPaymentAmount],
                         [IsFullyPaid],
                         [UserId],
                         [CreatedAt],
                         [CreatedBy],
                         [UpdatedAt],
                         [UpdatedBy])
                    VALUES
                        (
                            N'Housing Loan',
                            CAST(500000.00  AS DECIMAL(18, 2)),
                            CAST(4.00       AS DECIMAL(18, 2)),
                            CAST(359348.80  AS DECIMAL(18, 2)),
                            CAST(859348.80  AS DECIMAL(18, 2)),
                            CAST(859348.80  AS DECIMAL(18, 2)),
                            360,
                            360,
                            CAST(2387.08    AS DECIMAL(18, 2)),
                            CAST(0          AS BIT),
                            @userId,
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
                DELETE FROM [Performance].[Loans]
                WHERE [CreatedBy] = 'data seeding';
            ");
        }
    }
}