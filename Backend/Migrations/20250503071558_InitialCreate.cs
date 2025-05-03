using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Backend.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Monsters",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    Size = table.Column<int>(type: "integer", nullable: false),
                    Type = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    Alignment = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    ArmorClass = table.Column<int>(type: "integer", nullable: false),
                    HitPoints = table.Column<int>(type: "integer", nullable: false),
                    HitDice = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    Speed = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    Strength = table.Column<int>(type: "integer", nullable: false),
                    Dexterity = table.Column<int>(type: "integer", nullable: false),
                    Constitution = table.Column<int>(type: "integer", nullable: false),
                    Intelligence = table.Column<int>(type: "integer", nullable: false),
                    Wisdom = table.Column<int>(type: "integer", nullable: false),
                    Charisma = table.Column<int>(type: "integer", nullable: false),
                    SavingThrowStrength = table.Column<int>(type: "integer", nullable: false),
                    SavingThrowDexterity = table.Column<int>(type: "integer", nullable: false),
                    SavingThrowConstitution = table.Column<int>(type: "integer", nullable: false),
                    SavingThrowIntelligence = table.Column<int>(type: "integer", nullable: false),
                    SavingThrowWisdom = table.Column<int>(type: "integer", nullable: false),
                    SavingThrowCharisma = table.Column<int>(type: "integer", nullable: false),
                    Acrobatics = table.Column<int>(type: "integer", nullable: false),
                    AnimalHandling = table.Column<int>(type: "integer", nullable: false),
                    Arcana = table.Column<int>(type: "integer", nullable: false),
                    Athletics = table.Column<int>(type: "integer", nullable: false),
                    Deception = table.Column<int>(type: "integer", nullable: false),
                    History = table.Column<int>(type: "integer", nullable: false),
                    Insight = table.Column<int>(type: "integer", nullable: false),
                    Intimidation = table.Column<int>(type: "integer", nullable: false),
                    Investigation = table.Column<int>(type: "integer", nullable: false),
                    Medicine = table.Column<int>(type: "integer", nullable: false),
                    Nature = table.Column<int>(type: "integer", nullable: false),
                    Perception = table.Column<int>(type: "integer", nullable: false),
                    Performance = table.Column<int>(type: "integer", nullable: false),
                    Persuasion = table.Column<int>(type: "integer", nullable: false),
                    Religion = table.Column<int>(type: "integer", nullable: false),
                    SlightOfHand = table.Column<int>(type: "integer", nullable: false),
                    Stealth = table.Column<int>(type: "integer", nullable: false),
                    Survival = table.Column<int>(type: "integer", nullable: false),
                    DamageResistances = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    DamageImmunities = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    ConditionImmunities = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    Senses = table.Column<string>(type: "text", nullable: false),
                    Languages = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    ChallengeRating = table.Column<double>(type: "double precision", nullable: false),
                    Experience = table.Column<int>(type: "integer", nullable: false),
                    Actions = table.Column<string>(type: "text", nullable: false),
                    Image = table.Column<byte[]>(type: "bytea", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Monsters", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Email = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    Username = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Campaigns",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    GameMasterId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Campaigns", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Campaigns_Users_GameMasterId",
                        column: x => x.GameMasterId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CampaignUser",
                columns: table => new
                {
                    PlayerCampaignsId = table.Column<int>(type: "integer", nullable: false),
                    PlayersId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CampaignUser", x => new { x.PlayerCampaignsId, x.PlayersId });
                    table.ForeignKey(
                        name: "FK_CampaignUser_Campaigns_PlayerCampaignsId",
                        column: x => x.PlayerCampaignsId,
                        principalTable: "Campaigns",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CampaignUser_Users_PlayersId",
                        column: x => x.PlayersId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Characters",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    Image = table.Column<byte[]>(type: "bytea", nullable: false),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    CampaignId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Characters", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Characters_Campaigns_CampaignId",
                        column: x => x.CampaignId,
                        principalTable: "Campaigns",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Characters_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Maps",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    Name = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    Image = table.Column<byte[]>(type: "bytea", nullable: false),
                    IsGridActive = table.Column<bool>(type: "boolean", nullable: false),
                    GridSize = table.Column<int>(type: "integer", nullable: false),
                    Script = table.Column<string>(type: "text", nullable: false),
                    CampaignId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Maps", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Maps_Campaigns_CampaignId",
                        column: x => x.CampaignId,
                        principalTable: "Campaigns",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Tokens",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    X = table.Column<int>(type: "integer", nullable: false),
                    Y = table.Column<int>(type: "integer", nullable: false),
                    MapId = table.Column<int>(type: "integer", nullable: false),
                    OwnerId = table.Column<int>(type: "integer", nullable: false),
                    Discriminator = table.Column<string>(type: "character varying(21)", maxLength: 21, nullable: false),
                    CharacterId = table.Column<int>(type: "integer", nullable: true),
                    MonsterId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tokens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tokens_Characters_CharacterId",
                        column: x => x.CharacterId,
                        principalTable: "Characters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Tokens_Maps_MapId",
                        column: x => x.MapId,
                        principalTable: "Maps",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Tokens_Monsters_MonsterId",
                        column: x => x.MonsterId,
                        principalTable: "Monsters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Campaigns_GameMasterId",
                table: "Campaigns",
                column: "GameMasterId");

            migrationBuilder.CreateIndex(
                name: "IX_CampaignUser_PlayersId",
                table: "CampaignUser",
                column: "PlayersId");

            migrationBuilder.CreateIndex(
                name: "IX_Characters_CampaignId",
                table: "Characters",
                column: "CampaignId");

            migrationBuilder.CreateIndex(
                name: "IX_Characters_UserId",
                table: "Characters",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Maps_CampaignId",
                table: "Maps",
                column: "CampaignId");

            migrationBuilder.CreateIndex(
                name: "IX_Tokens_CharacterId",
                table: "Tokens",
                column: "CharacterId");

            migrationBuilder.CreateIndex(
                name: "IX_Tokens_MapId",
                table: "Tokens",
                column: "MapId");

            migrationBuilder.CreateIndex(
                name: "IX_Tokens_MonsterId",
                table: "Tokens",
                column: "MonsterId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CampaignUser");

            migrationBuilder.DropTable(
                name: "Tokens");

            migrationBuilder.DropTable(
                name: "Characters");

            migrationBuilder.DropTable(
                name: "Maps");

            migrationBuilder.DropTable(
                name: "Monsters");

            migrationBuilder.DropTable(
                name: "Campaigns");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
