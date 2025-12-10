using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Backend.Migrations
{
    /// <inheritdoc />
    public partial class CharacterStats : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Acrobatics",
                table: "Characters",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "AnimalHandling",
                table: "Characters",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Arcana",
                table: "Characters",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ArmorClass",
                table: "Characters",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Athletics",
                table: "Characters",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Attacks",
                table: "Characters",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Charisma",
                table: "Characters",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Class",
                table: "Characters",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Constitution",
                table: "Characters",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CurrentHitPoints",
                table: "Characters",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Deception",
                table: "Characters",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Dexterity",
                table: "Characters",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "EighthLevelSpellSlotTotal",
                table: "Characters",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "EighthLevelSpellSlotUsed",
                table: "Characters",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "FifthLevelSpellSlotTotal",
                table: "Characters",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "FifthLevelSpellSlotUsed",
                table: "Characters",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "FirstLevelSpellSlotTotal",
                table: "Characters",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "FirstLevelSpellSlotUsed",
                table: "Characters",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "FourthLevelSpellSlotTotal",
                table: "Characters",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "FourthLevelSpellSlotUsed",
                table: "Characters",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "History",
                table: "Characters",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "HitDice",
                table: "Characters",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "HitPoints",
                table: "Characters",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Insight",
                table: "Characters",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Intelligence",
                table: "Characters",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Intimidation",
                table: "Characters",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Inventory",
                table: "Characters",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Investigation",
                table: "Characters",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Level",
                table: "Characters",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Medicine",
                table: "Characters",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Nature",
                table: "Characters",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "NinthLevelSpellSlotTotal",
                table: "Characters",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "NinthLevelSpellSlotUsed",
                table: "Characters",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Perception",
                table: "Characters",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Performance",
                table: "Characters",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Persuasion",
                table: "Characters",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Race",
                table: "Characters",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Religion",
                table: "Characters",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SavingThrowCharisma",
                table: "Characters",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SavingThrowConstitution",
                table: "Characters",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SavingThrowDexterity",
                table: "Characters",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SavingThrowIntelligence",
                table: "Characters",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SavingThrowStrength",
                table: "Characters",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SavingThrowWisdom",
                table: "Characters",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SecondLevelSpellSlotTotal",
                table: "Characters",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SecondLevelSpellSlotUsed",
                table: "Characters",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SeventhLevelSpellSlotTotal",
                table: "Characters",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SeventhLevelSpellSlotUsed",
                table: "Characters",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SixthLevelSpellSlotTotal",
                table: "Characters",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SixthLevelSpellSlotUsed",
                table: "Characters",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SlightOfHand",
                table: "Characters",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Speed",
                table: "Characters",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Spells",
                table: "Characters",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Stealth",
                table: "Characters",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Strength",
                table: "Characters",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Survival",
                table: "Characters",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ThirdLevelSpellSlotTotal",
                table: "Characters",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ThirdLevelSpellSlotUsed",
                table: "Characters",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Traits",
                table: "Characters",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Wisdom",
                table: "Characters",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Acrobatics",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "AnimalHandling",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "Arcana",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "ArmorClass",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "Athletics",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "Attacks",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "Charisma",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "Class",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "Constitution",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "CurrentHitPoints",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "Deception",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "Dexterity",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "EighthLevelSpellSlotTotal",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "EighthLevelSpellSlotUsed",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "FifthLevelSpellSlotTotal",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "FifthLevelSpellSlotUsed",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "FirstLevelSpellSlotTotal",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "FirstLevelSpellSlotUsed",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "FourthLevelSpellSlotTotal",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "FourthLevelSpellSlotUsed",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "History",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "HitDice",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "HitPoints",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "Insight",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "Intelligence",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "Intimidation",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "Inventory",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "Investigation",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "Level",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "Medicine",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "Nature",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "NinthLevelSpellSlotTotal",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "NinthLevelSpellSlotUsed",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "Perception",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "Performance",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "Persuasion",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "Race",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "Religion",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "SavingThrowCharisma",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "SavingThrowConstitution",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "SavingThrowDexterity",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "SavingThrowIntelligence",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "SavingThrowStrength",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "SavingThrowWisdom",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "SecondLevelSpellSlotTotal",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "SecondLevelSpellSlotUsed",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "SeventhLevelSpellSlotTotal",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "SeventhLevelSpellSlotUsed",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "SixthLevelSpellSlotTotal",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "SixthLevelSpellSlotUsed",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "SlightOfHand",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "Speed",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "Spells",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "Stealth",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "Strength",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "Survival",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "ThirdLevelSpellSlotTotal",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "ThirdLevelSpellSlotUsed",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "Traits",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "Wisdom",
                table: "Characters");
        }
    }
}
