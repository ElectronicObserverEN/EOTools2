﻿// <auto-generated />
using System;
using EOToolsWeb.Api.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace EOToolsWeb.Api.Migrations
{
    [DbContext(typeof(EoToolsDbContext))]
    partial class EoToolsDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.7");

            modelBuilder.Entity("EOToolsWeb.Api.Models.UserConnection", b =>
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

            modelBuilder.Entity("EOToolsWeb.Api.Models.UserModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Users");
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

            modelBuilder.Entity("EOToolsWeb.Shared.ShipLocks.ShipLockModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasAnnotation("Relational:JsonPropertyName", "ToolsId");

                    b.Property<int>("ApiId")
                        .HasColumnType("INTEGER")
                        .HasAnnotation("Relational:JsonPropertyName", "Id");

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
                        .HasColumnType("TEXT")
                        .HasAnnotation("Relational:JsonPropertyName", "Name");

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

                    b.Property<string>("LockGroups")
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

                    b.Property<DateTimeOffset?>("UpdateDate")
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

            modelBuilder.Entity("EOToolsWeb.Api.Models.UserConnection", b =>
                {
                    b.HasOne("EOToolsWeb.Api.Models.UserModel", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("EOToolsWeb.Shared.Ships.ShipModel", b =>
                {
                    b.HasOne("EOToolsWeb.Shared.Ships.ShipClassModel", "ShipClass")
                        .WithMany()
                        .HasForeignKey("ShipClassId1");

                    b.Navigation("ShipClass");
                });
#pragma warning restore 612, 618
        }
    }
}
