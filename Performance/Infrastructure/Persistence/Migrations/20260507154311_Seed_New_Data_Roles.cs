using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Performance.Migrations
{
    /// <inheritdoc />
    public partial class Seed_New_Data_Roles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                IF EXISTS (
                    SELECT 1 FROM [Performance].[Roles]
                    WHERE [CreatedBy] = 'data seeding'
                )
                BEGIN
                    RETURN;
                END

                INSERT INTO [Performance].[Roles]
                    ([Name], [CreatedAt], [CreatedBy], [UpdatedAt], [UpdatedBy])
                VALUES
                    (N'admin', SYSDATETIMEOFFSET(), N'data seeding', SYSDATETIMEOFFSET(), N'data seeding'),
                    (N'user',  SYSDATETIMEOFFSET(), N'data seeding', SYSDATETIMEOFFSET(), N'data seeding');
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                DELETE FROM [Performance].[Roles]
                WHERE [CreatedBy] = 'data seeding';
            ");
        }
    }
}