﻿
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Users;

namespace Users.Migrations
{
    [DbContext(typeof(PostgresDbContext))]
    partial class PostgresDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 63)
                .HasAnnotation("ProductVersion", "5.0.17")
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            modelBuilder.Entity("Users.Model.User", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("text");

                    b.Property<string>("Email")
                        .HasColumnType("text");

                    b.Property<string>("FirstName")
                        .HasColumnType("text");

                    b.Property<string>("LastName")
                        .HasColumnType("text");

                    b.Property<string>("Password")
                        .HasColumnType("text");

                    b.Property<string>("Role")
                        .HasColumnType("text");

                    b.Property<string>("Username")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            Id = "1",
                            Email = "imeprezime@gmail.com",
                            FirstName = "Ime",
                            LastName = "Prezime",
                            Password = "sifra123",
                            Role = "HOST",
                            Username = "imeprezime"
                        },
                        new
                        {
                            Id = "2",
                            Email = "imenkoprezimenko@gmail.com",
                            FirstName = "Imenko",
                            LastName = "Prezimenic",
                            Password = "sifra123",
                            Role = "HOST",
                            Username = "imenkoprezimenko"
                        });
                });

            modelBuilder.Entity("Users.Model.User", b =>
                {
                    b.OwnsOne("Users.Model.Address", "Address", b1 =>
                        {
                            b1.Property<string>("UserId")
                                .HasColumnType("text");

                            b1.Property<string>("City")
                                .HasColumnType("text");

                            b1.Property<string>("Country")
                                .HasColumnType("text");

                            b1.Property<int>("Number")
                                .HasColumnType("integer");

                            b1.Property<string>("Street")
                                .HasColumnType("text");

                            b1.HasKey("UserId");

                            b1.ToTable("Users");

                            b1.WithOwner()
                                .HasForeignKey("UserId");
                        });

                    b.Navigation("Address");
                });
#pragma warning restore 612, 618
        }
    }
}
