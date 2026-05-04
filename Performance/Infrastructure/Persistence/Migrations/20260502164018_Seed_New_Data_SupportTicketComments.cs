using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Performance.Migrations
{
    /// <inheritdoc />
    public partial class Seed_New_Data_SupportTicketComments : Migration
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
                DELETE FROM [Performance].[SupportTicketComments]
                WHERE [CreatedBy] = 'data seeding';
            ");
        }

        private static string BuildBatchSql(int startId, int endId) => $@"
            IF NOT EXISTS (
                SELECT 1 FROM [Performance].[SupportTicketComments] STC
                INNER JOIN [Performance].[SupportTickets] ST ON ST.[Id]  = STC.[TicketId]
                INNER JOIN [Performance].[Users]          U  ON U.[Id]   = ST.[UserId]
                WHERE U.[Username] = N'user{startId}'
                AND STC.[CreatedBy] = 'data seeding'
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
                INSERT INTO [Performance].[SupportTicketComments]
                    ([CommentText], [RecommendedAction], [TicketId],
                    [CreatedAt], [CreatedBy], [UpdatedAt], [UpdatedBy])
                SELECT
                    CASE (N % 5)
                        WHEN 0 THEN N'Thank you for contacting support. We are looking into your issue.'
                        WHEN 1 THEN N'Our technical team is investigating this matter.'
                        WHEN 2 THEN N'We appreciate your patience while we resolve this issue.'
                        WHEN 3 THEN N'Your request has been escalated to our specialist team.'
                        WHEN 4 THEN N'We have reviewed your account and are working on a resolution.'
                    END,
                    CASE (N % 5)
                        WHEN 0 THEN N'Please verify your account details and try again.'
                        WHEN 1 THEN N'Try clearing your browser cache and logging in again.'
                        WHEN 2 THEN N'Please review the attached documentation for more information.'
                        WHEN 3 THEN N'Contact us at support@bank.com if the issue persists.'
                        WHEN 4 THEN N'We recommend waiting 24-48 hours for the transaction to reflect.'
                    END,
                    ST.[Id],
                    SYSDATETIMEOFFSET(),
                    N'data seeding',
                    SYSDATETIMEOFFSET(),
                    N'data seeding'
                FROM Tally
                INNER JOIN [Performance].[Users]          U  ON U.[Username] = N'user' + CAST(N AS NVARCHAR(10))
                INNER JOIN [Performance].[SupportTickets] ST ON ST.[UserId]  = U.[Id];
            END
            ";
    }
}