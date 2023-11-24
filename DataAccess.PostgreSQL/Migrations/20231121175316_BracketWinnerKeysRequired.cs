using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.PostgreSQL.Migrations
{
    /// <inheritdoc />
    public partial class BracketWinnerKeysRequired : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BracketWinners_Brackets_Id",
                table: "BracketWinners");

            migrationBuilder.AddForeignKey(
                name: "FK_BracketWinners_Brackets_Id",
                table: "BracketWinners",
                column: "Id",
                principalTable: "Brackets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BracketWinners_Brackets_Id",
                table: "BracketWinners");

            migrationBuilder.AddForeignKey(
                name: "FK_BracketWinners_Brackets_Id",
                table: "BracketWinners",
                column: "Id",
                principalTable: "Brackets",
                principalColumn: "Id");
        }
    }
}
