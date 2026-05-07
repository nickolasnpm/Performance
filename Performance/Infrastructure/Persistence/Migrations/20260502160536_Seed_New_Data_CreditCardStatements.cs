using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Performance.Migrations
{
    /// <inheritdoc />
    public partial class Seed_New_Data_CreditCardStatements : Migration
    {
        private const int totalData = 1_000_000;
        private const int totalPerBatch = 100_000;
        private const int startingNumber = 110_000;

        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                IF EXISTS (
                    SELECT 1 FROM [Performance].[CreditCardStatements]
                    WHERE [CreatedBy] = 'data seeding'
                )
                BEGIN
                    RETURN;
                END
            ");

            int batches = totalData / totalPerBatch;

            for (int batch = 0; batch < batches; batch++)
            {
                int startId = startingNumber + (batch * totalPerBatch);
                int endId = startId + totalPerBatch - 1;
                migrationBuilder.Sql(BuildBatchSql(startId, endId));
            }
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                DELETE FROM [Performance].[CreditCardStatements]
                WHERE [CreatedBy] = 'data seeding';
            ");
        }

        private static string BuildBatchSql(int startId, int endId) => $@"
            DECLARE @Now        DATETIMEOFFSET = SYSDATETIMEOFFSET();
            DECLARE @Today      DATE           = CAST(GETUTCDATE() AS DATE);
            DECLARE @NextMonth  DATE           = CAST(DATEADD(MONTH, 1, GETUTCDATE()) AS DATE);

            WITH
            E2(N) AS (
                SELECT 1 UNION ALL SELECT 2 UNION ALL SELECT 3 UNION ALL SELECT 4 UNION ALL SELECT 5
                UNION ALL SELECT 6 UNION ALL SELECT 7 UNION ALL SELECT 8 UNION ALL SELECT 9 UNION ALL SELECT 10
            ),
            E4(N) AS (SELECT 1 FROM E2 a CROSS JOIN E2 b),
            E6(N) AS (SELECT 1 FROM E4 a CROSS JOIN E4 b),
            E8(N) AS (SELECT 1 FROM E6 a CROSS JOIN E2 b),
            Tally(N) AS (
                SELECT TOP ({endId - startId + 1})
                    ROW_NUMBER() OVER (ORDER BY (SELECT NULL)) + {startId - 1}
                FROM E8
            )
            INSERT INTO [Performance].[CreditCardStatements]
                ([StatementDate], [DueDate], [StatementBalance], [MinimumPayment],
                [PaymentsReceived], [InterestCharged], [AvailableCredit],
                [CreditCardId], [CreatedAt], [CreatedBy], [UpdatedAt], [UpdatedBy])
            SELECT
                @Today,
                @NextMonth,
                CAST(1000 AS DECIMAL(18, 2)),
                CAST(50   AS DECIMAL(18, 2)),
                CAST(0    AS DECIMAL(18, 2)),
                CAST(0    AS DECIMAL(18, 2)),
                CAST(9000 AS DECIMAL(18, 2)),
                CC.[Id],
                @Now,
                N'data seeding',
                @Now,
                N'data seeding'
            FROM Tally
            INNER JOIN [Performance].[CreditCards] CC ON CC.[CardNumber] = N'4532' + RIGHT(REPLICATE(N'0', 12) + CAST(N AS NVARCHAR(12)), 12);
            ";
    }
}