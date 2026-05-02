using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Performance.Migrations
{
    /// <inheritdoc />
    public partial class Seed_New_Data_BankTransactions : Migration
    {
        private const int totalData = 1000000;
        private const int startingNumber = 110000;
        private readonly int endingNumber = totalData + startingNumber;

        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql($@"
                DECLARE @i              INT = {startingNumber};
                DECLARE @bankAccountId  BIGINT;         
                DECLARE @currentBalance DECIMAL(18, 2);

                WHILE @i < {endingNumber}
                BEGIN
                    -- Fetch BankAccountId and CurrentBalance in one go
                    SELECT
                        @bankAccountId  = [Id],
                        @currentBalance = [CurrentBalance]
                    FROM [Performance].[BankAccounts]
                    WHERE [AccountNumber] = N'ACC' + CAST(@i AS NVARCHAR(10));

                    INSERT INTO [Performance].[BankTransactions]
                        ([BaseAmount],
                         [FeeAmount],
                         [SettlementAmount],
                         [TransactionType],
                         [Status],
                         [MerchantName],
                         [ReferenceNumber],
                         [BankAccountId],
                         [CreatedAt],
                         [CreatedBy],
                         [UpdatedAt],
                         [UpdatedBy])
                    VALUES
                        (
                            @currentBalance,
                            CAST(0 AS DECIMAL(18, 2)),
                            @currentBalance,
                            1,
                            1,
                            N'Account Opening',
                            N'OPEN' + RIGHT(REPLICATE(N'0', 18) + CAST(@i AS NVARCHAR(18)), 18),
                            @bankAccountId,
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
                DELETE FROM [Performance].[BankTransactions]
                WHERE [CreatedBy] = 'data seeding';
            ");
        }
    }
}