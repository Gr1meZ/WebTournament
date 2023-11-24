using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.PostgreSQL.Migrations
{
    /// <inheritdoc />
    public partial class FighterBracket : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Brackets_WeightCategorieId",
                table: "Brackets",
                column: "WeightCategorieId");

            migrationBuilder.AddForeignKey(
                name: "FK_Brackets_WeightCategories_WeightCategorieId",
                table: "Brackets",
                column: "WeightCategorieId",
                principalTable: "WeightCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Brackets_WeightCategories_WeightCategorieId",
                table: "Brackets");

            migrationBuilder.DropIndex(
                name: "IX_Brackets_WeightCategorieId",
                table: "Brackets");
        }
    }
}
