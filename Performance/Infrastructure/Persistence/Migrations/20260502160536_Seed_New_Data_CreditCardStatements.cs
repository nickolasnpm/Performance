using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Performance.Migrations
{
    /// <inheritdoc />
    public partial class Seed_New_Data_CreditCardStatements : Migration
    {
        private const int totalData = 1000000;
        private const int startingNumber = 110000;
        private readonly int endingNumber = totalData + startingNumber;

        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql($@"
                DECLARE @i            INT = {startingNumber};
                DECLARE @creditCardId BIGINT;

                WHILE @i < {endingNumber}
                BEGIN
                    SELECT @creditCardId = [Id]
                    FROM [Performance].[CreditCards]
                    WHERE [CardNumber] = N'4532' + RIGHT(REPLICATE(N'0', 12) + CAST(@i AS NVARCHAR(12)), 12);

                    INSERT INTO [Performance].[CreditCardStatements]
                        ([StatementDate],
                         [DueDate],
                         [StatementBalance],
                         [MinimumPayment],
                         [PaymentsReceived],
                         [InterestCharged],
                         [AvailableCredit],
                         [CreditCardId],
                         [CreatedAt],
                         [CreatedBy],
                         [UpdatedAt],
                         [UpdatedBy])
                    VALUES
                        (
                            CAST(GETUTCDATE() AS DATE),
                            CAST(DATEADD(MONTH, 1, GETUTCDATE()) AS DATE),
                            CAST(1000 AS DECIMAL(18, 2)),
                            CAST(50 AS DECIMAL(18, 2)),
                            CAST(0 AS DECIMAL(18, 2)),
                            CAST(0 AS DECIMAL(18, 2)),
                            CAST(9000 AS DECIMAL(18, 2)),
                            @creditCardId,
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
                DELETE FROM [Performance].[CreditCardStatements]
                WHERE [CreatedBy] = 'data seeding';
            ");
        }
    }
}