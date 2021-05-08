using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ParkyAPI.Migrations
{
    public partial class AddUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "NationalParks",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Created", "Established" },
                values: new object[] { new DateTime(2020, 12, 2, 22, 21, 50, 974, DateTimeKind.Local).AddTicks(5857), new DateTime(2020, 12, 2, 22, 21, 50, 981, DateTimeKind.Local).AddTicks(3549) });

            migrationBuilder.UpdateData(
                table: "NationalParks",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Created", "Established" },
                values: new object[] { new DateTime(2020, 12, 2, 22, 21, 50, 981, DateTimeKind.Local).AddTicks(4357), new DateTime(2020, 12, 2, 22, 21, 50, 981, DateTimeKind.Local).AddTicks(4380) });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Password", "Role", "Username" },
                values: new object[] { 1, "123456aA", "Admin", "h4mmm1d" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.UpdateData(
                table: "NationalParks",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Created", "Established" },
                values: new object[] { new DateTime(2020, 12, 1, 1, 44, 1, 955, DateTimeKind.Local).AddTicks(7640), new DateTime(2020, 12, 1, 1, 44, 1, 965, DateTimeKind.Local).AddTicks(6088) });

            migrationBuilder.UpdateData(
                table: "NationalParks",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Created", "Established" },
                values: new object[] { new DateTime(2020, 12, 1, 1, 44, 1, 965, DateTimeKind.Local).AddTicks(7679), new DateTime(2020, 12, 1, 1, 44, 1, 965, DateTimeKind.Local).AddTicks(7732) });
        }
    }
}
