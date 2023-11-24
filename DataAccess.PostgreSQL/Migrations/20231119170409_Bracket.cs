using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.PostgreSQL.Migrations
{
    /// <inheritdoc />
    public partial class Bracket : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "BracketId",
                table: "Fighters",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "Brackets",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    WeightCategorieId = table.Column<Guid>(type: "uuid", nullable: false),
                    Division = table.Column<Guid[]>(type: "uuid[]", nullable: false),
                    State = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Brackets", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Fighters_BracketId",
                table: "Fighters",
                column: "BracketId");

            migrationBuilder.AddForeignKey(
                name: "FK_Fighters_Brackets_BracketId",
                table: "Fighters",
                column: "BracketId",
                principalTable: "Brackets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Fighters_Brackets_BracketId",
                table: "Fighters");

            migrationBuilder.DropTable(
                name: "Brackets");

            migrationBuilder.DropIndex(
                name: "IX_Fighters_BracketId",
                table: "Fighters");

            migrationBuilder.DropColumn(
                name: "BracketId",
                table: "Fighters");
        }
    }
}
