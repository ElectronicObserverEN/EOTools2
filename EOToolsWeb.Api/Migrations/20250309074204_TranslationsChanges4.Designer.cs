﻿// <auto-generated />
using System;
using EOToolsWeb.Api.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace EOToolsWeb.Api.Migrations
{
    [DbContext(typeof(EoToolsDbContext))]
    [Migration("20250309074204_TranslationsChanges4")]
    partial class TranslationsChanges4
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "9.0.0");

            modelBuilder.Entity("EOToolsWeb.Shared.ApplicationLog.DataChangedLogModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Changes")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("EntityId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("EntityName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("UserId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("DataChangeLogs");
                });

            modelBuilder.Entity("EOToolsWeb.Shared.EquipmentUpgrades.EquipmentUpgradeConversionModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("EquipmentLevelAfter")
                        .HasColumnType("INTEGER")
                        .HasAnnotation("Relational:JsonPropertyName", "lvl_after");

                    b.Property<int>("IdEquipmentAfter")
                        .HasColumnType("INTEGER")
                        .HasAnnotation("Relational:JsonPropertyName", "id_after");

                    b.Property<int>("ImprovmentModelId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("ImprovmentModelId")
                        .IsUnique();

                    b.ToTable("EquipmentUpgradeConversionModel");

                    b.HasAnnotation("Relational:JsonPropertyName", "convert");
                });

            modelBuilder.Entity("EOToolsWeb.Shared.EquipmentUpgrades.EquipmentUpgradeHelpersDayModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("Day")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("EquipmentUpgradeHelpersModelId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("EquipmentUpgradeHelpersModelId");

                    b.ToTable("EquipmentUpgradeHelpersDayModel");

                    b.HasAnnotation("Relational:JsonPropertyName", "days");
                });

            modelBuilder.Entity("EOToolsWeb.Shared.EquipmentUpgrades.EquipmentUpgradeHelpersModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("EquipmentUpgradeImprovmentModelId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("EquipmentUpgradeImprovmentModelId");

                    b.ToTable("EquipmentUpgradeHelpersModel");

                    b.HasAnnotation("Relational:JsonPropertyName", "helpers");
                });

            modelBuilder.Entity("EOToolsWeb.Shared.EquipmentUpgrades.EquipmentUpgradeHelpersShipModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int?>("EquipmentUpgradeHelpersModelId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("ShipId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("EquipmentUpgradeHelpersModelId");

                    b.ToTable("EquipmentUpgradeHelpersShipModel");

                    b.HasAnnotation("Relational:JsonPropertyName", "ship_ids");
                });

            modelBuilder.Entity("EOToolsWeb.Shared.EquipmentUpgrades.EquipmentUpgradeImprovmentCost", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("Ammo")
                        .HasColumnType("INTEGER")
                        .HasAnnotation("Relational:JsonPropertyName", "ammo");

                    b.Property<int>("Bauxite")
                        .HasColumnType("INTEGER")
                        .HasAnnotation("Relational:JsonPropertyName", "baux");

                    b.Property<int>("Cost0To5Id")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Cost6To9Id")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("CostMaxId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Fuel")
                        .HasColumnType("INTEGER")
                        .HasAnnotation("Relational:JsonPropertyName", "fuel");

                    b.Property<int>("Steel")
                        .HasColumnType("INTEGER")
                        .HasAnnotation("Relational:JsonPropertyName", "steel");

                    b.HasKey("Id");

                    b.HasIndex("Cost0To5Id");

                    b.HasIndex("Cost6To9Id");

                    b.HasIndex("CostMaxId");

                    b.ToTable("EquipmentUpgradeImprovmentCost");

                    b.HasAnnotation("Relational:JsonPropertyName", "costs");
                });

            modelBuilder.Entity("EOToolsWeb.Shared.EquipmentUpgrades.EquipmentUpgradeImprovmentCostDetail", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("DevmatCost")
                        .HasColumnType("INTEGER")
                        .HasAnnotation("Relational:JsonPropertyName", "devmats");

                    b.Property<int>("ImproveMatCost")
                        .HasColumnType("INTEGER")
                        .HasAnnotation("Relational:JsonPropertyName", "screws");

                    b.Property<int>("SliderDevmatCost")
                        .HasColumnType("INTEGER")
                        .HasAnnotation("Relational:JsonPropertyName", "devmats_sli");

                    b.Property<int>("SliderImproveMatCost")
                        .HasColumnType("INTEGER")
                        .HasAnnotation("Relational:JsonPropertyName", "screws_sli");

                    b.HasKey("Id");

                    b.ToTable("EquipmentUpgradeImprovmentCostDetail");

                    b.HasAnnotation("Relational:JsonPropertyName", "conv");
                });

            modelBuilder.Entity("EOToolsWeb.Shared.EquipmentUpgrades.EquipmentUpgradeImprovmentCostItemDetail", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("Count")
                        .HasColumnType("INTEGER")
                        .HasAnnotation("Relational:JsonPropertyName", "eq_count");

                    b.Property<int?>("EquipmentUpgradeImprovmentCostDetailId")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("EquipmentUpgradeImprovmentCostDetailId1")
                        .HasColumnType("INTEGER");

                    b.Property<int>("ItemId")
                        .HasColumnType("INTEGER")
                        .HasAnnotation("Relational:JsonPropertyName", "id");

                    b.HasKey("Id");

                    b.HasIndex("EquipmentUpgradeImprovmentCostDetailId");

                    b.HasIndex("EquipmentUpgradeImprovmentCostDetailId1");

                    b.ToTable("EquipmentUpgradeImprovmentCostItemDetail");

                    b.HasAnnotation("Relational:JsonPropertyName", "equips");
                });

            modelBuilder.Entity("EOToolsWeb.Shared.EquipmentUpgrades.EquipmentUpgradeImprovmentModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("CostsId")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("EquipmentUpgradeModelId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("CostsId");

                    b.HasIndex("EquipmentUpgradeModelId");

                    b.ToTable("Improvments");
                });

            modelBuilder.Entity("EOToolsWeb.Shared.EquipmentUpgrades.EquipmentUpgradeModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("EquipmentId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("EquipmentUpgradeDataModel");
                });

            modelBuilder.Entity("EOToolsWeb.Shared.Equipments.EquipmentModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasAnnotation("Relational:JsonPropertyName", "id");

                    b.Property<int>("ApiId")
                        .HasColumnType("INTEGER")
                        .HasAnnotation("Relational:JsonPropertyName", "api_id");

                    b.Property<bool>("CanBeCrafted")
                        .HasColumnType("INTEGER")
                        .HasAnnotation("Relational:JsonPropertyName", "development");

                    b.Property<string>("NameEN")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasAnnotation("Relational:JsonPropertyName", "name_en");

                    b.Property<string>("NameJP")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasAnnotation("Relational:JsonPropertyName", "name_jp");

                    b.HasKey("Id");

                    b.ToTable("Equipments");
                });

            modelBuilder.Entity("EOToolsWeb.Shared.Equipments.EquipmentTranslationModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("EquipmentId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("EquipmentTranslations");
                });

            modelBuilder.Entity("EOToolsWeb.Shared.Events.EventModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasAnnotation("Relational:JsonPropertyName", "id");

                    b.Property<int>("ApiId")
                        .HasColumnType("INTEGER")
                        .HasAnnotation("Relational:JsonPropertyName", "api_id");

                    b.Property<int?>("EndOnUpdateId")
                        .HasColumnType("INTEGER")
                        .HasAnnotation("Relational:JsonPropertyName", "end_update_id");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasAnnotation("Relational:JsonPropertyName", "name");

                    b.Property<int?>("StartOnUpdateId")
                        .HasColumnType("INTEGER")
                        .HasAnnotation("Relational:JsonPropertyName", "start_update_id");

                    b.HasKey("Id");

                    b.ToTable("Events");
                });

            modelBuilder.Entity("EOToolsWeb.Shared.Maps.FleetNameTranslationModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("Fleets");
                });

            modelBuilder.Entity("EOToolsWeb.Shared.Maps.MapNameTranslationModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("Maps");
                });

            modelBuilder.Entity("EOToolsWeb.Shared.Quests.QuestDescriptionTranslationModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("QuestId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("QuestDescriptionTranslations");
                });

            modelBuilder.Entity("EOToolsWeb.Shared.Quests.QuestModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasAnnotation("Relational:JsonPropertyName", "id");

                    b.Property<int?>("AddedOnUpdateId")
                        .HasColumnType("INTEGER")
                        .HasAnnotation("Relational:JsonPropertyName", "added_update_id");

                    b.Property<int>("ApiId")
                        .HasColumnType("INTEGER")
                        .HasAnnotation("Relational:JsonPropertyName", "api_id");

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasAnnotation("Relational:JsonPropertyName", "code");

                    b.Property<string>("DescEN")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasAnnotation("Relational:JsonPropertyName", "desc_en");

                    b.Property<string>("DescJP")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasAnnotation("Relational:JsonPropertyName", "desc_jp");

                    b.Property<string>("NameEN")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasAnnotation("Relational:JsonPropertyName", "name_en");

                    b.Property<string>("NameJP")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasAnnotation("Relational:JsonPropertyName", "name_jp");

                    b.Property<int?>("RemovedOnUpdateId")
                        .HasColumnType("INTEGER")
                        .HasAnnotation("Relational:JsonPropertyName", "removed_update_id");

                    b.Property<int?>("SeasonId")
                        .HasColumnType("INTEGER")
                        .HasAnnotation("Relational:JsonPropertyName", "season_id");

                    b.Property<string>("Tracker")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasAnnotation("Relational:JsonPropertyName", "tracker");

                    b.HasKey("Id");

                    b.HasIndex("Code", "ApiId")
                        .IsUnique();

                    b.ToTable("Quests");
                });

            modelBuilder.Entity("EOToolsWeb.Shared.Quests.QuestTitleTranslationModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("QuestId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("QuestTitleTranslations");
                });

            modelBuilder.Entity("EOToolsWeb.Shared.Seasons.SeasonModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasAnnotation("Relational:JsonPropertyName", "id");

                    b.Property<int?>("AddedOnUpdateId")
                        .HasColumnType("INTEGER")
                        .HasAnnotation("Relational:JsonPropertyName", "added_update_id");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasAnnotation("Relational:JsonPropertyName", "name");

                    b.Property<int?>("RemovedOnUpdateId")
                        .HasColumnType("INTEGER")
                        .HasAnnotation("Relational:JsonPropertyName", "removed_update_id");

                    b.HasKey("Id");

                    b.ToTable("Seasons");
                });

            modelBuilder.Entity("EOToolsWeb.Shared.ShipLocks.ShipLockModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("ApiId")
                        .HasColumnType("INTEGER");

                    b.Property<byte>("ColorA")
                        .HasColumnType("INTEGER")
                        .HasAnnotation("Relational:JsonPropertyName", "A");

                    b.Property<byte>("ColorB")
                        .HasColumnType("INTEGER")
                        .HasAnnotation("Relational:JsonPropertyName", "B");

                    b.Property<byte>("ColorG")
                        .HasColumnType("INTEGER")
                        .HasAnnotation("Relational:JsonPropertyName", "G");

                    b.Property<byte>("ColorR")
                        .HasColumnType("INTEGER")
                        .HasAnnotation("Relational:JsonPropertyName", "R");

                    b.Property<int>("EventId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("NameEnglish")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("NameJapanese")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Locks");
                });

            modelBuilder.Entity("EOToolsWeb.Shared.ShipLocks.ShipLockPhaseModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("EventId")
                        .HasColumnType("INTEGER");

                    b.PrimitiveCollection<string>("LockGroups")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("PhaseName")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasAnnotation("Relational:JsonPropertyName", "Name");

                    b.Property<int>("SortId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("LockPhases");
                });

            modelBuilder.Entity("EOToolsWeb.Shared.Ships.ShipClassModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("ApiId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("NameEnglish")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("NameJapanese")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("ShipClasses");
                });

            modelBuilder.Entity("EOToolsWeb.Shared.Ships.ShipModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("ApiId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("NameEN")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("NameJP")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int?>("ShipClassId1")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("ShipClassId1");

                    b.ToTable("Ships");
                });

            modelBuilder.Entity("EOToolsWeb.Shared.Ships.ShipNameTranslationModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("ShipTranslations");
                });

            modelBuilder.Entity("EOToolsWeb.Shared.Ships.ShipSuffixTranslationModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("ShipSuffixTranslations");
                });

            modelBuilder.Entity("EOToolsWeb.Shared.Translations.TranslationModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int?>("EquipmentTranslationModelId")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("FleetNameTranslationModelId")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsPendingChange")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Language")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("MapNameTranslationModelId")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("QuestDescriptionTranslationModelId")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("QuestTitleTranslationModelId")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("ShipNameTranslationModelId")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("ShipSuffixTranslationModelId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Translation")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("EquipmentTranslationModelId");

                    b.HasIndex("FleetNameTranslationModelId");

                    b.HasIndex("MapNameTranslationModelId");

                    b.HasIndex("QuestDescriptionTranslationModelId");

                    b.HasIndex("QuestTitleTranslationModelId");

                    b.HasIndex("ShipNameTranslationModelId");

                    b.HasIndex("ShipSuffixTranslationModelId");

                    b.ToTable("TranslationModel");
                });

            modelBuilder.Entity("EOToolsWeb.Shared.Updates.UpdateModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasAnnotation("Relational:JsonPropertyName", "id");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasAnnotation("Relational:JsonPropertyName", "description");

                    b.Property<string>("EndTweetLink")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasAnnotation("Relational:JsonPropertyName", "maint_end_tweet");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasAnnotation("Relational:JsonPropertyName", "name");

                    b.Property<string>("StartTweetLink")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasAnnotation("Relational:JsonPropertyName", "maint_start_tweet");

                    b.Property<DateTime?>("UpdateDate")
                        .HasColumnType("TEXT")
                        .HasAnnotation("Relational:JsonPropertyName", "start_date");

                    b.Property<TimeSpan?>("UpdateEndTime")
                        .HasColumnType("TEXT")
                        .HasAnnotation("Relational:JsonPropertyName", "end_time");

                    b.Property<TimeSpan?>("UpdateStartTime")
                        .HasColumnType("TEXT")
                        .HasAnnotation("Relational:JsonPropertyName", "start_time");

                    b.Property<bool>("WasLiveUpdate")
                        .HasColumnType("INTEGER")
                        .HasAnnotation("Relational:JsonPropertyName", "live_update");

                    b.HasKey("Id");

                    b.ToTable("Updates");
                });

            modelBuilder.Entity("EOToolsWeb.Shared.Users.UserConnection", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Token")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("UserId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("UserConnections");
                });

            modelBuilder.Entity("EOToolsWeb.Shared.Users.UserModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("Kind")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("Username")
                        .IsUnique();

                    b.ToTable("Users");
                });

            modelBuilder.Entity("EOToolsWeb.Shared.ApplicationLog.DataChangedLogModel", b =>
                {
                    b.HasOne("EOToolsWeb.Shared.Users.UserModel", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("EOToolsWeb.Shared.EquipmentUpgrades.EquipmentUpgradeConversionModel", b =>
                {
                    b.HasOne("EOToolsWeb.Shared.EquipmentUpgrades.EquipmentUpgradeImprovmentModel", "ImprovmentModel")
                        .WithOne("ConversionData")
                        .HasForeignKey("EOToolsWeb.Shared.EquipmentUpgrades.EquipmentUpgradeConversionModel", "ImprovmentModelId")
                        .OnDelete(DeleteBehavior.ClientCascade)
                        .IsRequired();

                    b.Navigation("ImprovmentModel");
                });

            modelBuilder.Entity("EOToolsWeb.Shared.EquipmentUpgrades.EquipmentUpgradeHelpersDayModel", b =>
                {
                    b.HasOne("EOToolsWeb.Shared.EquipmentUpgrades.EquipmentUpgradeHelpersModel", null)
                        .WithMany("CanHelpOnDays")
                        .HasForeignKey("EquipmentUpgradeHelpersModelId");
                });

            modelBuilder.Entity("EOToolsWeb.Shared.EquipmentUpgrades.EquipmentUpgradeHelpersModel", b =>
                {
                    b.HasOne("EOToolsWeb.Shared.EquipmentUpgrades.EquipmentUpgradeImprovmentModel", "Improvment")
                        .WithMany("Helpers")
                        .HasForeignKey("EquipmentUpgradeImprovmentModelId")
                        .OnDelete(DeleteBehavior.ClientCascade)
                        .IsRequired();

                    b.Navigation("Improvment");
                });

            modelBuilder.Entity("EOToolsWeb.Shared.EquipmentUpgrades.EquipmentUpgradeHelpersShipModel", b =>
                {
                    b.HasOne("EOToolsWeb.Shared.EquipmentUpgrades.EquipmentUpgradeHelpersModel", null)
                        .WithMany("ShipIds")
                        .HasForeignKey("EquipmentUpgradeHelpersModelId");
                });

            modelBuilder.Entity("EOToolsWeb.Shared.EquipmentUpgrades.EquipmentUpgradeImprovmentCost", b =>
                {
                    b.HasOne("EOToolsWeb.Shared.EquipmentUpgrades.EquipmentUpgradeImprovmentCostDetail", "Cost0To5")
                        .WithMany()
                        .HasForeignKey("Cost0To5Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("EOToolsWeb.Shared.EquipmentUpgrades.EquipmentUpgradeImprovmentCostDetail", "Cost6To9")
                        .WithMany()
                        .HasForeignKey("Cost6To9Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("EOToolsWeb.Shared.EquipmentUpgrades.EquipmentUpgradeImprovmentCostDetail", "CostMax")
                        .WithMany()
                        .HasForeignKey("CostMaxId");

                    b.Navigation("Cost0To5");

                    b.Navigation("Cost6To9");

                    b.Navigation("CostMax");
                });

            modelBuilder.Entity("EOToolsWeb.Shared.EquipmentUpgrades.EquipmentUpgradeImprovmentCostItemDetail", b =>
                {
                    b.HasOne("EOToolsWeb.Shared.EquipmentUpgrades.EquipmentUpgradeImprovmentCostDetail", null)
                        .WithMany("ConsumableDetail")
                        .HasForeignKey("EquipmentUpgradeImprovmentCostDetailId");

                    b.HasOne("EOToolsWeb.Shared.EquipmentUpgrades.EquipmentUpgradeImprovmentCostDetail", null)
                        .WithMany("EquipmentDetail")
                        .HasForeignKey("EquipmentUpgradeImprovmentCostDetailId1");
                });

            modelBuilder.Entity("EOToolsWeb.Shared.EquipmentUpgrades.EquipmentUpgradeImprovmentModel", b =>
                {
                    b.HasOne("EOToolsWeb.Shared.EquipmentUpgrades.EquipmentUpgradeImprovmentCost", "Costs")
                        .WithMany()
                        .HasForeignKey("CostsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("EOToolsWeb.Shared.EquipmentUpgrades.EquipmentUpgradeModel", null)
                        .WithMany("Improvement")
                        .HasForeignKey("EquipmentUpgradeModelId");

                    b.Navigation("Costs");
                });

            modelBuilder.Entity("EOToolsWeb.Shared.Ships.ShipModel", b =>
                {
                    b.HasOne("EOToolsWeb.Shared.Ships.ShipClassModel", "ShipClass")
                        .WithMany()
                        .HasForeignKey("ShipClassId1");

                    b.Navigation("ShipClass");
                });

            modelBuilder.Entity("EOToolsWeb.Shared.Translations.TranslationModel", b =>
                {
                    b.HasOne("EOToolsWeb.Shared.Equipments.EquipmentTranslationModel", null)
                        .WithMany("Translations")
                        .HasForeignKey("EquipmentTranslationModelId");

                    b.HasOne("EOToolsWeb.Shared.Maps.FleetNameTranslationModel", null)
                        .WithMany("Translations")
                        .HasForeignKey("FleetNameTranslationModelId");

                    b.HasOne("EOToolsWeb.Shared.Maps.MapNameTranslationModel", null)
                        .WithMany("Translations")
                        .HasForeignKey("MapNameTranslationModelId");

                    b.HasOne("EOToolsWeb.Shared.Quests.QuestDescriptionTranslationModel", null)
                        .WithMany("Translations")
                        .HasForeignKey("QuestDescriptionTranslationModelId");

                    b.HasOne("EOToolsWeb.Shared.Quests.QuestTitleTranslationModel", null)
                        .WithMany("Translations")
                        .HasForeignKey("QuestTitleTranslationModelId");

                    b.HasOne("EOToolsWeb.Shared.Ships.ShipNameTranslationModel", null)
                        .WithMany("Translations")
                        .HasForeignKey("ShipNameTranslationModelId");

                    b.HasOne("EOToolsWeb.Shared.Ships.ShipSuffixTranslationModel", null)
                        .WithMany("Translations")
                        .HasForeignKey("ShipSuffixTranslationModelId");
                });

            modelBuilder.Entity("EOToolsWeb.Shared.Users.UserConnection", b =>
                {
                    b.HasOne("EOToolsWeb.Shared.Users.UserModel", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("EOToolsWeb.Shared.EquipmentUpgrades.EquipmentUpgradeHelpersModel", b =>
                {
                    b.Navigation("CanHelpOnDays");

                    b.Navigation("ShipIds");
                });

            modelBuilder.Entity("EOToolsWeb.Shared.EquipmentUpgrades.EquipmentUpgradeImprovmentCostDetail", b =>
                {
                    b.Navigation("ConsumableDetail");

                    b.Navigation("EquipmentDetail");
                });

            modelBuilder.Entity("EOToolsWeb.Shared.EquipmentUpgrades.EquipmentUpgradeImprovmentModel", b =>
                {
                    b.Navigation("ConversionData");

                    b.Navigation("Helpers");
                });

            modelBuilder.Entity("EOToolsWeb.Shared.EquipmentUpgrades.EquipmentUpgradeModel", b =>
                {
                    b.Navigation("Improvement");
                });

            modelBuilder.Entity("EOToolsWeb.Shared.Equipments.EquipmentTranslationModel", b =>
                {
                    b.Navigation("Translations");
                });

            modelBuilder.Entity("EOToolsWeb.Shared.Maps.FleetNameTranslationModel", b =>
                {
                    b.Navigation("Translations");
                });

            modelBuilder.Entity("EOToolsWeb.Shared.Maps.MapNameTranslationModel", b =>
                {
                    b.Navigation("Translations");
                });

            modelBuilder.Entity("EOToolsWeb.Shared.Quests.QuestDescriptionTranslationModel", b =>
                {
                    b.Navigation("Translations");
                });

            modelBuilder.Entity("EOToolsWeb.Shared.Quests.QuestTitleTranslationModel", b =>
                {
                    b.Navigation("Translations");
                });

            modelBuilder.Entity("EOToolsWeb.Shared.Ships.ShipNameTranslationModel", b =>
                {
                    b.Navigation("Translations");
                });

            modelBuilder.Entity("EOToolsWeb.Shared.Ships.ShipSuffixTranslationModel", b =>
                {
                    b.Navigation("Translations");
                });
#pragma warning restore 612, 618
        }
    }
}
