using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Performance.Migrations
{
    /// <inheritdoc />
    public partial class Seed_New_Data_BankAccounts : Migration
    {
        private const int totalData = 1000000;
        private const int startingNumber = 110000;
        private readonly int endingNumber = totalData + startingNumber;

        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql($@"
                DECLARE @i INT = {startingNumber};

                WHILE @i < {endingNumber}
                BEGIN
                    INSERT INTO [Performance].[BankAccounts]
                        ([AccountNumber],
                         [CurrentBalance],
                         [AvailableBalance],
                         [UserId],
                         [CreatedAt],
                         [CreatedBy],
                         [UpdatedAt],
                         [UpdatedBy])
                    VALUES
                        (
                            N'ACC' + CAST(@i AS NVARCHAR(10)),
                            CAST((@i % 100000) + 1 AS DECIMAL(18, 2)),
                            CAST((@i % 100000) + 1 AS DECIMAL(18, 2)),
                            (SELECT [Id] FROM [Performance].[Users] WHERE [Username] = N'user' + CAST(@i AS NVARCHAR(10))),
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
                DELETE FROM [Performance].[BankAccounts]
                WHERE [CreatedBy] = 'data seeding';
            ");
        }
    }
}