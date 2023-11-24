using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.PostgreSQL.Migrations
{
    /// <inheritdoc />
    public partial class BracketWinner : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BracketWinners",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    FirstPlaceId = table.Column<Guid>(type: "uuid", nullable: false),
                    SecondPlaceId = table.Column<Guid>(type: "uuid", nullable: false),
                    ThirdPlaceId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BracketWinners", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BracketWinners_Brackets_Id",
                        column: x => x.Id,
                        principalTable: "Brackets",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_BracketWinners_Fighters_FirstPlaceId",
                        column: x => x.FirstPlaceId,
                        principalTable: "Fighters",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_BracketWinners_Fighters_SecondPlaceId",
                        column: x => x.SecondPlaceId,
                        principalTable: "Fighters",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_BracketWinners_Fighters_ThirdPlaceId",
                        column: x => x.ThirdPlaceId,
                        principalTable: "Fighters",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_BracketWinners_FirstPlaceId",
                table: "BracketWinners",
                column: "FirstPlaceId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BracketWinners_SecondPlaceId",
                table: "BracketWinners",
                column: "SecondPlaceId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BracketWinners_ThirdPlaceId",
                table: "BracketWinners",
                column: "ThirdPlaceId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BracketWinners");
        }
    }
}
