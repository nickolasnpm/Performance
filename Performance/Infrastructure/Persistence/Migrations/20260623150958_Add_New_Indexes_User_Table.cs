using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Performance.Migrations
{
    /// <inheritdoc />
    public partial class Add_New_Indexes_User_Table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Users_FirstName",
                schema: "Performance",
                table: "Users",
                column: "FirstName");

            migrationBuilder.CreateIndex(
                name: "IX_Users_LastName",
                schema: "Performance",
                table: "Users",
                column: "LastName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Users_FirstName",
                schema: "Performance",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_LastName",
                schema: "Performance",
                table: "Users");
        }
    }
}
