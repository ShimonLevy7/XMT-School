using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TestsManagerDatabase.Migrations
{
    /// <inheritdoc />
    public partial class DbContextV9 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SelectedAnswers_Marks_MarkId",
                table: "SelectedAnswers");

            migrationBuilder.AlterColumn<int>(
                name: "MarkId",
                table: "SelectedAnswers",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_SelectedAnswers_Marks_MarkId",
                table: "SelectedAnswers",
                column: "MarkId",
                principalTable: "Marks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SelectedAnswers_Marks_MarkId",
                table: "SelectedAnswers");

            migrationBuilder.AlterColumn<int>(
                name: "MarkId",
                table: "SelectedAnswers",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_SelectedAnswers_Marks_MarkId",
                table: "SelectedAnswers",
                column: "MarkId",
                principalTable: "Marks",
                principalColumn: "Id");
        }
    }
}
