﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Server.Models;

#nullable disable

namespace Server.Migrations
{
    [DbContext(typeof(SQLDatabase))]
    partial class SQLDatabaseModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.1");

            modelBuilder.Entity("Server.Models.DbActiveCampaignElements", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("AmbientId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("CampaignId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("EffectId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("MapId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("ActiveCampaignElements");
                });

            modelBuilder.Entity("Server.Models.DbCampaign", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Campaigns");
                });

            modelBuilder.Entity("Server.Models.DbCharacter", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("Charisma")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Class")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Constitution")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Dexterity")
                        .HasColumnType("INTEGER");

                    b.Property<int>("ExperiencePoints")
                        .HasColumnType("INTEGER");

                    b.Property<byte[]>("Image")
                        .IsRequired()
                        .HasColumnType("BLOB");

                    b.Property<int>("Intelligence")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("Race")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Strength")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Wisdom")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("Characters");
                });

            modelBuilder.Entity("Server.Models.DbCharactersInCampaign", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("CampaignId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("CharacterId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("UserId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("CharactersInCampaign");
                });

            modelBuilder.Entity("Server.Models.DbDiceRoll", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("CampaignId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Roll")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("DiceRolls");
                });

            modelBuilder.Entity("Server.Models.DbMap", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("CampaignId")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("GridIsActive")
                        .HasColumnType("INTEGER");

                    b.Property<int>("GridSize")
                        .HasColumnType("INTEGER");

                    b.Property<byte[]>("ImageData")
                        .IsRequired()
                        .HasColumnType("BLOB");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Script")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Maps");
                });

            modelBuilder.Entity("Server.Models.DbMonster", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("Acrobatics")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Actions")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Alignment")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("AnimalHandling")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Arcana")
                        .HasColumnType("INTEGER");

                    b.Property<int>("ArmorClass")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Athletics")
                        .HasColumnType("INTEGER");

                    b.Property<double>("ChallangeRating")
                        .HasColumnType("REAL");

                    b.Property<int>("Charisma")
                        .HasColumnType("INTEGER");

                    b.Property<string>("ConditionImmunities")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("Constitution")
                        .HasColumnType("INTEGER");

                    b.Property<string>("DamageImmunities")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("DamageResistances")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("Deception")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Dexterity")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Experience")
                        .HasColumnType("INTEGER");

                    b.Property<int>("History")
                        .HasColumnType("INTEGER");

                    b.Property<string>("HitDice")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("HitPoints")
                        .HasColumnType("INTEGER");

                    b.Property<byte[]>("Image")
                        .IsRequired()
                        .HasColumnType("BLOB");

                    b.Property<int>("Insight")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Intelligence")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Intimidation")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Investigation")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Languages")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("Medicine")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("Nature")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Perception")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Performance")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Persuasion")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Religion")
                        .HasColumnType("INTEGER");

                    b.Property<int>("SavingThrowCharisma")
                        .HasColumnType("INTEGER");

                    b.Property<int>("SavingThrowConstitution")
                        .HasColumnType("INTEGER");

                    b.Property<int>("SavingThrowDexterity")
                        .HasColumnType("INTEGER");

                    b.Property<int>("SavingThrowIntelligence")
                        .HasColumnType("INTEGER");

                    b.Property<int>("SavingThrowStrength")
                        .HasColumnType("INTEGER");

                    b.Property<int>("SavingThrowWisdom")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Senses")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("Size")
                        .HasColumnType("INTEGER");

                    b.Property<int>("SlightOfHand")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Speed")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("Stealth")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Strength")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Survival")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("Wisdom")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("Monsters");
                });

            modelBuilder.Entity("Server.Models.DbSound", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<byte[]>("Data")
                        .IsRequired()
                        .HasColumnType("BLOB");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Tags")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("Type")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("Sounds");
                });

            modelBuilder.Entity("Server.Models.DbToken", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int?>("CharacterId")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("MonsterId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("X")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Y")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("Tokens");
                });

            modelBuilder.Entity("Server.Models.DbTokensOnMap", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("MapId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("TokenId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("TokensOnMap");
                });

            modelBuilder.Entity("Server.Models.DbUser", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Server.Models.DbUserInCampaign", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("CampaignId")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsGamemaster")
                        .HasColumnType("INTEGER");

                    b.Property<int>("UserId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("UsersInCampaign");
                });
#pragma warning restore 612, 618
        }
    }
}
