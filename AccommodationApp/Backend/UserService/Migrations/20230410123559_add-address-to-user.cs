using Microsoft.EntityFrameworkCore.Migrations;

namespace Users.Migrations
{
    public partial class addaddresstouser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "1");

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "2");

            migrationBuilder.AddColumn<string>(
                name: "Address_City",
                table: "Users",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Address_Country",
                table: "Users",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Address_Number",
                table: "Users",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Address_Street",
                table: "Users",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address_City",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Address_Country",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Address_Number",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Address_Street",
                table: "Users");

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "FirstName", "LastName", "Password", "Role", "Username" },
                values: new object[,]
                {
                    { "1", "imeprezime@gmail.com", "Ime", "Prezime", "sifra123", "HOST", "imeprezime" },
                    { "2", "imenkoprezimenko@gmail.com", "Imenko", "Prezimenic", "sifra123", "HOST", "imenkoprezimenko" }
                });
        }
    }
}
