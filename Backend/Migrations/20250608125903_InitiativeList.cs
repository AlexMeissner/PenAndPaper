using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Backend.Migrations
{
    /// <inheritdoc />
    public partial class InitiativeList : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "Initiative",
                table: "Tokens",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ActingTokenId",
                table: "Maps",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Maps_ActingTokenId",
                table: "Maps",
                column: "ActingTokenId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Maps_Tokens_ActingTokenId",
                table: "Maps",
                column: "ActingTokenId",
                principalTable: "Tokens",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Maps_Tokens_ActingTokenId",
                table: "Maps");

            migrationBuilder.DropIndex(
                name: "IX_Maps_ActingTokenId",
                table: "Maps");

            migrationBuilder.DropColumn(
                name: "Initiative",
                table: "Tokens");

            migrationBuilder.DropColumn(
                name: "ActingTokenId",
                table: "Maps");
        }
    }
}
