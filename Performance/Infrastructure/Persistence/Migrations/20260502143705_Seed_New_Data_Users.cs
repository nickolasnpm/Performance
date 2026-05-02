using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Performance.Migrations
{
    /// <inheritdoc />
    public partial class Seed_New_Data_Users : Migration
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
                    INSERT INTO [Performance].[Users]
                        ([Username],
                         [Email],
                         [FirstName],
                         [LastName],
                         [DateOfBirth],
                         [PhoneNumber],
                         [ProfilePictureUrl],
                         [IsEmailVerified],
                         [IsActive],
                         [LastLoginAt],
                         [CreatedAt],
                         [CreatedBy],
                         [UpdatedAt],
                         [UpdatedBy])
                    VALUES
                        (
                            N'user'  + CAST(@i AS NVARCHAR(10)),
                            N'user'  + CAST(@i AS NVARCHAR(10)) + N'@example.com',
                            N'First' + CAST(@i AS NVARCHAR(10)),
                            N'Last'  + CAST(@i AS NVARCHAR(10)),
                            CAST('2000-10-10' AS DATE),
                            N'1234567890',
                            NULL,
                            CAST(0 AS BIT),
                            CAST(0 AS BIT),
                            NULL,
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
                DELETE FROM [Performance].[Users]
                WHERE [CreatedBy] = 'data seeding';
            ");
        }
    }
}