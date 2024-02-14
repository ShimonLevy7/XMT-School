using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TestsManagerDatabase.Migrations
{
    /// <inheritdoc />
    public partial class DbContextV8 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Marks_Users_UserId",
                table: "Marks");

            migrationBuilder.AddForeignKey(
                name: "FK_Marks_Users_UserId",
                table: "Marks",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Marks_Users_UserId",
                table: "Marks");

            migrationBuilder.AddForeignKey(
                name: "FK_Marks_Users_UserId",
                table: "Marks",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
