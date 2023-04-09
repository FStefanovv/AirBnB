using Microsoft.EntityFrameworkCore.Migrations;

namespace Users.Migrations
{
    public partial class seedonceagain : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "FirstName", "LastName", "Password", "Role", "Username" },
                values: new object[] { "2", "imenkoprezimenko@gmail.com", "Imenko", "Prezimenic", "sifra123", "HOST", "imenkoprezimenko" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "2");
        }
    }
}
