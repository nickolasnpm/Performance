using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Performance.Migrations
{
    /// <inheritdoc />
    public partial class Seed_New_Data_Users : Migration
    {
        private const int totalData = 1_000_000;
        private const int totalPerBatch = 100_000;
        private const int startingNumber = 110_000;

        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                IF EXISTS (
                    SELECT 1 FROM [Performance].[Users]
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
                DELETE FROM [Performance].[Users]
                WHERE [CreatedBy] = 'data seeding';
            ");
        }

        private static string BuildBatchSql(int startId, int endId) => $@"
            DECLARE @Now DATETIMEOFFSET = SYSDATETIMEOFFSET();

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
            INSERT INTO [Performance].[Users]
                ([Username], [Email], [FirstName], [LastName],
                [DateOfBirth], [PhoneNumber], [ProfilePictureUrl],
                [IsEmailVerified], [IsActive], [LastLoginAt],
                [CreatedAt], [CreatedBy], [UpdatedAt], [UpdatedBy])
            SELECT
                N'user'  + CAST(N AS NVARCHAR(10)),
                N'user'  + CAST(N AS NVARCHAR(10)) + N'@example.com',
                N'First' + CAST(N AS NVARCHAR(10)),
                N'Last'  + CAST(N AS NVARCHAR(10)),
                CAST('2000-10-10' AS DATE),
                N'1234567890',
                NULL,
                CAST(0 AS BIT),
                CAST(0 AS BIT),
                NULL,
                @Now,
                N'data seeding',
                @Now,
                N'data seeding'
            FROM Tally;
            ";
    }
}