using Microsoft.EntityFrameworkCore.Migrations;

namespace Users.Migrations
{
    public partial class newMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDistinguishedHost",
                table: "Users",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsRatingPartSatisfied",
                table: "Users",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsReservationPartSatisfied",
                table: "Users",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDistinguishedHost",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "IsRatingPartSatisfied",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "IsReservationPartSatisfied",
                table: "Users");
        }
    }
}
