using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server.Migrations
{
    /// <inheritdoc />
    public partial class Settings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Setting",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    DiceSuccessSoundId = table.Column<int>(type: "INTEGER", nullable: true),
                    DiceFailSoundId = table.Column<int>(type: "INTEGER", nullable: true),
                    DiceCritSuccessSoundId = table.Column<int>(type: "INTEGER", nullable: true),
                    DiceCritFailSoundId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Setting", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Setting_Sounds_DiceCritFailSoundId",
                        column: x => x.DiceCritFailSoundId,
                        principalTable: "Sounds",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Setting_Sounds_DiceCritSuccessSoundId",
                        column: x => x.DiceCritSuccessSoundId,
                        principalTable: "Sounds",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Setting_Sounds_DiceFailSoundId",
                        column: x => x.DiceFailSoundId,
                        principalTable: "Sounds",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Setting_Sounds_DiceSuccessSoundId",
                        column: x => x.DiceSuccessSoundId,
                        principalTable: "Sounds",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Setting_DiceCritFailSoundId",
                table: "Setting",
                column: "DiceCritFailSoundId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Setting_DiceCritSuccessSoundId",
                table: "Setting",
                column: "DiceCritSuccessSoundId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Setting_DiceFailSoundId",
                table: "Setting",
                column: "DiceFailSoundId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Setting_DiceSuccessSoundId",
                table: "Setting",
                column: "DiceSuccessSoundId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Setting");
        }
    }
}
