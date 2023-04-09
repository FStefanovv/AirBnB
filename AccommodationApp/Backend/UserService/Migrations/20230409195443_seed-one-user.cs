using Microsoft.EntityFrameworkCore.Migrations;

namespace Users.Migrations
{
    public partial class seedoneuser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "FirstName", "LastName", "Password", "Role", "Username" },
                values: new object[] { "1", "imeprezime@gmail.com", "Ime", "Prezime", "sifra123", "HOST", "imeprezime" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "1");
        }
    }
}
