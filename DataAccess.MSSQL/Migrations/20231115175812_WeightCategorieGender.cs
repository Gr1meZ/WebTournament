using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.MSSQL.Migrations
{
    /// <inheritdoc />
    public partial class WeightCategorieGender : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Gender",
                table: "WeightCategories",
                type: "int",
                nullable: false,
                defaultValue: 0);
            
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Gender",
                table: "WeightCategories");
            
        }
    }
}
