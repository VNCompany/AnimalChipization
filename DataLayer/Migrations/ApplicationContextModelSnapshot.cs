﻿// <auto-generated />
using System;
using DataLayer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DataLayer.Migrations
{
    [DbContext(typeof(ApplicationContext))]
    partial class ApplicationContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("DataLayer.Entities.Account", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Role")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasColumnType("text")
                        .HasDefaultValue("USER");

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.ToTable("Accounts");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Email = "admin@simbirsoft.com",
                            FirstName = "adminFirstName",
                            LastName = "adminLastName",
                            Password = "daaad6e5604e8e17bd9f108d91e26afe6281dac8fda0091040a7a6d7bd9b43b5",
                            Role = "ADMIN"
                        },
                        new
                        {
                            Id = 2,
                            Email = "chipper@simbirsoft.com",
                            FirstName = "chipperFirstName",
                            LastName = "chipperLastName",
                            Password = "daaad6e5604e8e17bd9f108d91e26afe6281dac8fda0091040a7a6d7bd9b43b5",
                            Role = "CHIPPER"
                        },
                        new
                        {
                            Id = 3,
                            Email = "user@simbirsoft.com",
                            FirstName = "userFirstName",
                            LastName = "userLastName",
                            Password = "daaad6e5604e8e17bd9f108d91e26afe6281dac8fda0091040a7a6d7bd9b43b5",
                            Role = "USER"
                        });
                });

            modelBuilder.Entity("DataLayer.Entities.Animal", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<int>("ChipperId")
                        .HasColumnType("integer");

                    b.Property<DateTime?>("ChippingDateTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<long>("ChippingLocationId")
                        .HasColumnType("bigint");

                    b.Property<DateTime?>("DeathDateTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Gender")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<decimal>("Height")
                        .HasColumnType("NUMERIC(13,8)");

                    b.Property<decimal>("Length")
                        .HasColumnType("NUMERIC(13,8)");

                    b.Property<string>("LifeStatus")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasColumnType("text")
                        .HasDefaultValue("ALIVE");

                    b.Property<decimal>("Weight")
                        .HasColumnType("NUMERIC(13,8)");

                    b.HasKey("Id");

                    b.HasIndex("ChipperId");

                    b.HasIndex("ChippingLocationId");

                    b.ToTable("Animals");
                });

            modelBuilder.Entity("DataLayer.Entities.AnimalType", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("AnimalTypes");
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
                });

            modelBuilder.Entity("DataLayer.Entities.Area", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<string>("Geometry")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Areas");
                });

            modelBuilder.Entity("DataLayer.Entities.LocationPoint", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<double>("Latitude")
                        .HasColumnType("double precision");

                    b.Property<double>("Longitude")
                        .HasColumnType("double precision");

                    b.HasKey("Id");

                    b.ToTable("LocationPoints");
                });

            modelBuilder.Entity("DataLayer.Entities.VisitedLocation", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<long>("AnimalId")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("DateTimeOfVisitLocationPoint")
                        .HasColumnType("timestamp with time zone");

                    b.Property<long>("LocationPointId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("AnimalId");

                    b.HasIndex("LocationPointId");

                    b.ToTable("VisitedLocations");
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
