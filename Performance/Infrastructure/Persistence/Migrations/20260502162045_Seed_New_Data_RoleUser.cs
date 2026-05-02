using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Performance.Migrations
{
    /// <inheritdoc />
    public partial class Seed_New_Data_RoleUser : Migration
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

                    INSERT INTO [Performance].[RoleUser]
                        ([RolesId],
                         [UsersId])
                    VALUES
                        (
                            -- RoleId: always 2
                            2,

                            @userId
                        );

                    SET @i = @i + 1;
                END
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql($@"
                DELETE RU
                FROM [Performance].[RoleUser] RU
                INNER JOIN [Performance].[Users] U ON U.[Id] = RU.[UsersId]
                WHERE U.[CreatedBy] = 'data seeding';
            ");
        }
    }
}