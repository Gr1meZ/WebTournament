using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.PostgreSQL.Migrations
{
    /// <inheritdoc />
    public partial class TournamentBracket : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "TournamentId",
                table: "Brackets",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Brackets_TournamentId",
                table: "Brackets",
                column: "TournamentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Brackets_Tournaments_TournamentId",
                table: "Brackets",
                column: "TournamentId",
                principalTable: "Tournaments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Brackets_Tournaments_TournamentId",
                table: "Brackets");

            migrationBuilder.DropIndex(
                name: "IX_Brackets_TournamentId",
                table: "Brackets");

            migrationBuilder.DropColumn(
                name: "TournamentId",
                table: "Brackets");
        }
    }
}
