using Microsoft.EntityFrameworkCore.Migrations;

namespace Users.Migrations
{
    public partial class _1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    FirstName = table.Column<string>(type: "text", nullable: true),
                    LastName = table.Column<string>(type: "text", nullable: true),
                    Email = table.Column<string>(type: "text", nullable: true),
                    Username = table.Column<string>(type: "text", nullable: true),
                    Password = table.Column<string>(type: "text", nullable: true),
                    Role = table.Column<string>(type: "text", nullable: true),
                    Address_Country = table.Column<string>(type: "text", nullable: true),
                    Address_City = table.Column<string>(type: "text", nullable: true),
                    Address_Street = table.Column<string>(type: "text", nullable: true),
                    Address_Number = table.Column<int>(type: "integer", nullable: true),
                    IsDistinguishedHost = table.Column<bool>(type: "boolean", nullable: false),
                    IsReservationPartSatisfied = table.Column<bool>(type: "boolean", nullable: false),
                    IsRatingPartSatisfied = table.Column<bool>(type: "boolean", nullable: false),
                    State = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "FirstName", "IsDistinguishedHost", "IsRatingPartSatisfied", "IsReservationPartSatisfied", "LastName", "Password", "Role", "State", "Username" },
                values: new object[,]
                {
                    { "1", "imeprezime@gmail.com", "Ime", false, false, false, "Prezime", "sifra123", "HOST", 0, "imeprezime" },
                    { "2", "imenkoprezimenko@gmail.com", "Imenko", false, false, false, "Prezimenic", "sifra123", "HOST", 0, "imenkoprezimenko" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
