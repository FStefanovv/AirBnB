﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using ReservationService;

namespace ReservationService.Migrations
{
    [DbContext(typeof(PostgresDbContext))]
    [Migration("20230505101323_initialSetup")]
    partial class initialSetup
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 63)
                .HasAnnotation("ProductVersion", "5.0.17")
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            modelBuilder.Entity("ReservationService.Model.Reservation", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("text");

                    b.Property<string>("AccommodationId")
                        .HasColumnType("text");

                    b.Property<string>("AccommodationName")
                        .HasColumnType("text");

                    b.Property<DateTime>("From")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("HostId")
                        .HasColumnType("text");

                    b.Property<int>("NumberOfGuests")
                        .HasColumnType("integer");

                    b.Property<double>("Price")
                        .HasColumnType("double precision");

                    b.Property<int>("Status")
                        .HasColumnType("integer");

                    b.Property<DateTime>("To")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("UserId")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Reservations");

                    b.HasData(
                        new
                        {
                            Id = "1",
                            AccommodationId = "643455b8941a49684fb8cfc1",
                            From = new DateTime(2023, 5, 20, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            NumberOfGuests = 10,
                            Price = 0.0,
                            Status = 0,
                            To = new DateTime(2023, 5, 28, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            UserId = "7bf59c03-17a6-49a9-92f1-4c251b0d0bd8"
                        });
                });

            modelBuilder.Entity("ReservationService.Model.ReservationRequest", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("text");

                    b.Property<string>("AccommodationId")
                        .HasColumnType("text");

                    b.Property<string>("AccommodationName")
                        .HasColumnType("text");

                    b.Property<DateTime>("From")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("HostId")
                        .HasColumnType("text");

                    b.Property<int>("NumberOfGuests")
                        .HasColumnType("integer");

                    b.Property<int>("Status")
                        .HasColumnType("integer");

                    b.Property<DateTime>("To")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("UserId")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Requests");

                    b.HasData(
                        new
                        {
                            Id = "1",
                            AccommodationId = "643455b8941a49684fb8cfc1",
                            From = new DateTime(2023, 5, 10, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            NumberOfGuests = 12,
                            Status = 0,
                            To = new DateTime(2023, 5, 15, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            UserId = "f6b022d5-15bf-44d9-9217-e542e68585a0"
                        },
                        new
                        {
                            Id = "2",
                            AccommodationId = "643455b8941a49684fb8cfc1",
                            From = new DateTime(2023, 5, 20, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            NumberOfGuests = 10,
                            Status = 0,
                            To = new DateTime(2023, 5, 28, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            UserId = "7bf59c03-17a6-49a9-92f1-4c251b0d0bd8"
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
