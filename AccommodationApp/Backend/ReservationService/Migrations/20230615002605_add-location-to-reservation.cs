using Microsoft.EntityFrameworkCore.Migrations;

namespace ReservationService.Migrations
{
    public partial class addlocationtoreservation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AccommodationLocaiton",
                table: "Reservations",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccommodationLocaiton",
                table: "Reservations");
        }
    }
}
