using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Performance.Migrations
{
    /// <inheritdoc />
    public partial class Seed_New_Data_SupportTickets : Migration
    {
        private const int totalData      = 1_000_000;
        private const int totalPerBatch  = 10_000;
        private const int startingNumber = 110_000;

        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            int batches = totalData / totalPerBatch;

            for (int batch = 0; batch < batches; batch++)
            {
                int startId = startingNumber + (batch * totalPerBatch);
                int endId   = startId + totalPerBatch - 1;
                migrationBuilder.Sql(BuildBatchSql(startId, endId));
            }
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                DELETE FROM [Performance].[SupportTickets]
                WHERE [CreatedBy] = 'data seeding';
            ");
        }

        private static string BuildBatchSql(int startId, int endId) => $@"
            IF NOT EXISTS (
                SELECT 1 FROM [Performance].[SupportTickets] ST
                INNER JOIN [Performance].[Users] U ON U.[Id] = ST.[UserId]
                WHERE U.[Username] = N'user{startId}'
                AND ST.[CreatedBy] = 'data seeding'
            )
            BEGIN
                WITH
                E2(N) AS (
                    SELECT 1 UNION ALL SELECT 2 UNION ALL SELECT 3 UNION ALL SELECT 4 UNION ALL SELECT 5
                    UNION ALL SELECT 6 UNION ALL SELECT 7 UNION ALL SELECT 8 UNION ALL SELECT 9 UNION ALL SELECT 10
                ),
                E4(N) AS (SELECT 1 FROM E2 a CROSS JOIN E2 b),
                E6(N) AS (SELECT 1 FROM E4 a CROSS JOIN E4 b),
                Tally(N) AS (
                    SELECT TOP ({endId - startId + 1})
                        ROW_NUMBER() OVER (ORDER BY (SELECT NULL)) + {startId - 1}
                    FROM E6
                )
                INSERT INTO [Performance].[SupportTickets]
                    ([Subject], [Description], [Priority], [IsResolved],
                    [UserId], [CreatedAt], [CreatedBy], [UpdatedAt], [UpdatedBy])
                SELECT
                    CASE (N % 5)
                        WHEN 0 THEN N'Loan repayment query'
                        WHEN 1 THEN N'Update contact information'
                        WHEN 2 THEN N'Transaction not reflected in statement'
                        WHEN 3 THEN N'Unable to access my account'
                        WHEN 4 THEN N'Credit card payment issue'
                    END,
                    CASE (N % 5)
                        WHEN 0 THEN N'I need clarification on my loan repayment schedule and interest calculation.REF' + RIGHT(REPLICATE(N'0', 14) + CAST(N AS NVARCHAR(14)), 14)
                        WHEN 1 THEN N'I would like to update my phone number and email address on file.REF' + RIGHT(REPLICATE(N'0', 14) + CAST(N AS NVARCHAR(14)), 14)
                        WHEN 2 THEN N'My recent transaction is not showing in my bank statement. Transaction reference: REF' + RIGHT(REPLICATE(N'0', 14) + CAST(N AS NVARCHAR(14)), 14)
                        WHEN 3 THEN N'I am unable to login to my account. Please help me reset my password.REF' + RIGHT(REPLICATE(N'0', 14) + CAST(N AS NVARCHAR(14)), 14)
                        WHEN 4 THEN N'I made a credit card payment but it has not been processed yet.REF' + RIGHT(REPLICATE(N'0', 14) + CAST(N AS NVARCHAR(14)), 14)
                    END,
                    (N % 3) + 1,
                    CAST(0 AS BIT),
                    U.[Id],
                    SYSDATETIMEOFFSET(),
                    N'data seeding',
                    SYSDATETIMEOFFSET(),
                    N'data seeding'
                FROM Tally
                INNER JOIN [Performance].[Users] U
                    ON U.[Username] = N'user' + CAST(N AS NVARCHAR(10));
            END
            ";
    }
}