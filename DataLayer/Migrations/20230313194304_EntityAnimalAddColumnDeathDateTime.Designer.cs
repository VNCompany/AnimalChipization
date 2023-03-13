﻿// <auto-generated />
using System;
using DataLayer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace DataLayer.Migrations
{
    [DbContext(typeof(ApplicationContext))]
    [Migration("20230313194304_EntityAnimalAddColumnDeathDateTime")]
    partial class EntityAnimalAddColumnDeathDateTime
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("DataLayer.Entities.Account", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.ToTable("Accounts");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Email = "victor@mail.ru",
                            FirstName = "Victor",
                            LastName = "Neznanov",
                            Password = "99bde068af2d49ed7fc8b8fa79abe13a6059e0db320bb73459fd96624bb4b33f"
                        },
                        new
                        {
                            Id = 2,
                            Email = "anton@mail.ru",
                            FirstName = "Anton",
                            LastName = "Belousov",
                            Password = "1f29f2d29f02f2608eb72d45625ba3a851eda1ee2be1bda22427a584b787c722"
                        });
                });

            modelBuilder.Entity("DataLayer.Entities.Animal", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    b.Property<int>("ChipperId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("ChippingDateTime")
                        .HasColumnType("datetime(6)");

                    b.Property<long>("ChippingLocationId")
                        .HasColumnType("bigint");

                    b.Property<DateTime?>("DeathDateTime")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Gender")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<float>("Height")
                        .HasColumnType("float");

                    b.Property<float>("Length")
                        .HasColumnType("float");

                    b.Property<string>("LifeStatus")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasColumnType("longtext")
                        .HasDefaultValue("ALIVE");

                    b.Property<float>("Weight")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.HasIndex("ChipperId");

                    b.HasIndex("ChippingLocationId");

                    b.ToTable("Animals");

                    b.HasData(
                        new
                        {
                            Id = 1L,
                            ChipperId = 1,
                            ChippingDateTime = new DateTime(2022, 7, 13, 12, 23, 54, 0, DateTimeKind.Unspecified),
                            ChippingLocationId = 1L,
                            Gender = "MALE",
                            Height = 95.7f,
                            Length = 185.5f,
                            LifeStatus = "ALIVE",
                            Weight = 200.2f
                        });
                });

            modelBuilder.Entity("DataLayer.Entities.AnimalType", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("AnimalTypes");

                    b.HasData(
                        new
                        {
                            Id = 1L,
                            Type = "elephant"
                        },
                        new
                        {
                            Id = 2L,
                            Type = "monkey"
                        });
                });

            modelBuilder.Entity("DataLayer.Entities.AnimalsTypesLink", b =>
                {
                    b.Property<long>("AnimalId")
                        .HasColumnType("bigint");

                    b.Property<long>("AnimalTypeId")
                        .HasColumnType("bigint");

                    b.HasKey("AnimalId", "AnimalTypeId");

                    b.HasIndex("AnimalTypeId");

                    b.ToTable("AnimalsTypesLinks");

                    b.HasData(
                        new
                        {
                            AnimalId = 1L,
                            AnimalTypeId = 2L
                        });
                });

            modelBuilder.Entity("DataLayer.Entities.LocationPoint", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    b.Property<double>("Latitude")
                        .HasColumnType("double");

                    b.Property<double>("Longitude")
                        .HasColumnType("double");

                    b.HasKey("Id");

                    b.ToTable("LocationPoints");

                    b.HasData(
                        new
                        {
                            Id = 1L,
                            Latitude = 56.195,
                            Longitude = 23.121200000000002
                        },
                        new
                        {
                            Id = 2L,
                            Latitude = 55.112400000000001,
                            Longitude = 134.56729999999999
                        });
                });

            modelBuilder.Entity("DataLayer.Entities.VisitedLocation", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    b.Property<long>("AnimalId")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("DateTimeOfVisitLocationPoint")
                        .HasColumnType("datetime(6)");

                    b.Property<long>("LocationPointId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("AnimalId");

                    b.HasIndex("LocationPointId");

                    b.ToTable("VisitedLocations");

                    b.HasData(
                        new
                        {
                            Id = 1L,
                            AnimalId = 1L,
                            DateTimeOfVisitLocationPoint = new DateTime(2023, 2, 14, 13, 59, 33, 0, DateTimeKind.Local),
                            LocationPointId = 1L
                        });
                });

            modelBuilder.Entity("DataLayer.Entities.Animal", b =>
                {
                    b.HasOne("DataLayer.Entities.Account", "Chipper")
                        .WithMany()
                        .HasForeignKey("ChipperId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DataLayer.Entities.LocationPoint", "ChippingLocation")
                        .WithMany()
                        .HasForeignKey("ChippingLocationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Chipper");

                    b.Navigation("ChippingLocation");
                });

            modelBuilder.Entity("DataLayer.Entities.AnimalsTypesLink", b =>
                {
                    b.HasOne("DataLayer.Entities.Animal", "Animal")
                        .WithMany("AnimalsTypesLinks")
                        .HasForeignKey("AnimalId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DataLayer.Entities.AnimalType", "AnimalType")
                        .WithMany()
                        .HasForeignKey("AnimalTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Animal");

                    b.Navigation("AnimalType");
                });

            modelBuilder.Entity("DataLayer.Entities.VisitedLocation", b =>
                {
                    b.HasOne("DataLayer.Entities.Animal", "Animal")
                        .WithMany("VisitedLocations")
                        .HasForeignKey("AnimalId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DataLayer.Entities.LocationPoint", "LocationPoint")
                        .WithMany()
                        .HasForeignKey("LocationPointId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Animal");

                    b.Navigation("LocationPoint");
                });

            modelBuilder.Entity("DataLayer.Entities.Animal", b =>
                {
                    b.Navigation("AnimalsTypesLinks");

                    b.Navigation("VisitedLocations");
                });
#pragma warning restore 612, 618
        }
    }
}
