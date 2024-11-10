﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using OT.Assessment.Migrations;

#nullable disable

namespace OT.Assessment.Migrations.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20241110114726_AddedEntitiesMapping")]
    partial class AddedEntitiesMapping
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("OT.Assessment.Model.Entities.CasinoWager", b =>
                {
                    b.Property<Guid>("WagerId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("AccountId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<decimal>("Amount")
                        .HasPrecision(18, 4)
                        .HasColumnType("decimal(18,4)");

                    b.Property<Guid>("BrandId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<long>("Duration")
                        .HasColumnType("bigint");

                    b.Property<Guid?>("ExternalReferenceId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("GameId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("LastModifiedDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("NumberOfBets")
                        .HasColumnType("int");

                    b.Property<string>("SessionData")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("TransactionId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("TransactionTypeId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("WagerId");

                    b.HasIndex("Amount");

                    b.HasIndex("GameId");

                    b.HasIndex("TransactionId");

                    b.HasIndex("AccountId", "CreatedDate");

                    b.ToTable("CasinoWagers");
                });

            modelBuilder.Entity("OT.Assessment.Model.Entities.Game", b =>
                {
                    b.Property<Guid>("GameId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("GameName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<DateTime>("LastModifiedDate")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("ProviderId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Theme")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("GameId");

                    b.HasIndex("ProviderId", "GameName")
                        .IsUnique();

                    b.ToTable("Games");
                });

            modelBuilder.Entity("OT.Assessment.Model.Entities.Player", b =>
                {
                    b.Property<Guid>("AccountId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("CountryCode")
                        .HasMaxLength(2)
                        .HasColumnType("nvarchar(2)");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("LastModifiedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("AccountId");

                    b.HasIndex("Username")
                        .IsUnique();

                    b.ToTable("Players");
                });

            modelBuilder.Entity("OT.Assessment.Model.Entities.PlayerStats", b =>
                {
                    b.Property<Guid>("AccountId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("LastCalculatedDateTime")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("LastModifiedDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("LastWagerDateTime")
                        .HasColumnType("datetime2");

                    b.Property<decimal>("TotalAmount")
                        .HasPrecision(18, 4)
                        .HasColumnType("decimal(18,4)");

                    b.Property<int>("WagerCount")
                        .HasColumnType("int");

                    b.HasKey("AccountId");

                    b.HasIndex("TotalAmount");

                    b.ToTable("PlayerStats");
                });

            modelBuilder.Entity("OT.Assessment.Model.Entities.Provider", b =>
                {
                    b.Property<Guid>("ProviderId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("LastModifiedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("ProviderName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("ProviderId");

                    b.HasIndex("ProviderName")
                        .IsUnique();

                    b.ToTable("Providers");
                });

            modelBuilder.Entity("OT.Assessment.Model.Entities.CasinoWager", b =>
                {
                    b.HasOne("OT.Assessment.Model.Entities.Player", "Player")
                        .WithMany("Wagers")
                        .HasForeignKey("AccountId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("OT.Assessment.Model.Entities.Game", "Game")
                        .WithMany("Wagers")
                        .HasForeignKey("GameId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Game");

                    b.Navigation("Player");
                });

            modelBuilder.Entity("OT.Assessment.Model.Entities.Game", b =>
                {
                    b.HasOne("OT.Assessment.Model.Entities.Provider", "Provider")
                        .WithMany("Games")
                        .HasForeignKey("ProviderId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Provider");
                });

            modelBuilder.Entity("OT.Assessment.Model.Entities.PlayerStats", b =>
                {
                    b.HasOne("OT.Assessment.Model.Entities.Player", "Player")
                        .WithOne("PlayerStats")
                        .HasForeignKey("OT.Assessment.Model.Entities.PlayerStats", "AccountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Player");
                });

            modelBuilder.Entity("OT.Assessment.Model.Entities.Game", b =>
                {
                    b.Navigation("Wagers");
                });

            modelBuilder.Entity("OT.Assessment.Model.Entities.Player", b =>
                {
                    b.Navigation("PlayerStats");

                    b.Navigation("Wagers");
                });

            modelBuilder.Entity("OT.Assessment.Model.Entities.Provider", b =>
                {
                    b.Navigation("Games");
                });
#pragma warning restore 612, 618
        }
    }
}
