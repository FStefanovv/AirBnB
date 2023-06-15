using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ReservationService.Migrations
{
    public partial class DBInit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Requests",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    From = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    To = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UserId = table.Column<string>(type: "text", nullable: true),
                    AccommodationId = table.Column<string>(type: "text", nullable: true),
                    AccommodationName = table.Column<string>(type: "text", nullable: true),
                    HostId = table.Column<string>(type: "text", nullable: true),
                    NumberOfGuests = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Requests", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Reservations",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    From = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    To = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UserId = table.Column<string>(type: "text", nullable: true),
                    AccommodationId = table.Column<string>(type: "text", nullable: true),
                    AccommodationName = table.Column<string>(type: "text", nullable: true),
                    HostId = table.Column<string>(type: "text", nullable: true),
                    NumberOfGuests = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    Price = table.Column<double>(type: "double precision", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reservations", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Requests",
                columns: new[] { "Id", "AccommodationId", "AccommodationName", "From", "HostId", "NumberOfGuests", "Status", "To", "UserId" },
                values: new object[,]
                {
                    { "1", "643455b8941a49684fb8cfc1", null, new DateTime(2023, 5, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 12, 0, new DateTime(2023, 5, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "f6b022d5-15bf-44d9-9217-e542e68585a0" },
                    { "2", "643455b8941a49684fb8cfc1", null, new DateTime(2023, 5, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 10, 0, new DateTime(2023, 5, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), "7bf59c03-17a6-49a9-92f1-4c251b0d0bd8" }
                });

            migrationBuilder.InsertData(
                table: "Reservations",
                columns: new[] { "Id", "AccommodationId", "AccommodationName", "From", "HostId", "NumberOfGuests", "Price", "Status", "To", "UserId" },
                values: new object[] { "1", "643455b8941a49684fb8cfc1", null, new DateTime(2023, 5, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 10, 0.0, 0, new DateTime(2023, 5, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), "7bf59c03-17a6-49a9-92f1-4c251b0d0bd8" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Requests");

            migrationBuilder.DropTable(
                name: "Reservations");
        }
    }
}
