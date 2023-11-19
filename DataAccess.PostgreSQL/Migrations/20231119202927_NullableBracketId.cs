using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.PostgreSQL.Migrations
{
    /// <inheritdoc />
    public partial class NullableBracketId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Fighters_Brackets_BracketId",
                table: "Fighters");

            migrationBuilder.AlterColumn<Guid>(
                name: "BracketId",
                table: "Fighters",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddForeignKey(
                name: "FK_Fighters_Brackets_BracketId",
                table: "Fighters",
                column: "BracketId",
                principalTable: "Brackets",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Fighters_Brackets_BracketId",
                table: "Fighters");

            migrationBuilder.AlterColumn<Guid>(
                name: "BracketId",
                table: "Fighters",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Fighters_Brackets_BracketId",
                table: "Fighters",
                column: "BracketId",
                principalTable: "Brackets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
