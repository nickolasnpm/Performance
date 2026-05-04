using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Performance.Migrations
{
    /// <inheritdoc />
    public partial class Seed_New_Data_RoleUser : Migration
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
                DELETE RU
                FROM [Performance].[RoleUser] RU
                INNER JOIN [Performance].[Users] U ON U.[Id] = RU.[UsersId]
                WHERE U.[CreatedBy] = 'data seeding';
            ");
        }

        private static string BuildBatchSql(int startId, int endId) => $@"
            IF NOT EXISTS (
                SELECT 1 FROM [Performance].[RoleUser] RU
                INNER JOIN [Performance].[Users] U ON U.[Id] = RU.[UsersId]
                WHERE U.[Username] = N'user{startId}'
                AND RU.[RolesId] = 2
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
                INSERT INTO [Performance].[RoleUser]
                    ([RolesId], [UsersId])
                SELECT
                    2,
                    U.[Id]
                FROM Tally
                INNER JOIN [Performance].[Users] U
                    ON U.[Username] = N'user' + CAST(N AS NVARCHAR(10));
            END
            ";
    }
}