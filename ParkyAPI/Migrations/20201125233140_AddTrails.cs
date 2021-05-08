using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ParkyAPI.Migrations
{
    public partial class AddTrails : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Trails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Distance = table.Column<double>(type: "float", nullable: false),
                    Difficulty = table.Column<byte>(type: "tinyint", nullable: false),
                    NationalParkId = table.Column<int>(type: "int", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Trails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Trails_NationalParks_NationalParkId",
                        column: x => x.NationalParkId,
                        principalTable: "NationalParks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "NationalParks",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Created", "Established" },
                values: new object[] { new DateTime(2020, 11, 26, 3, 1, 39, 538, DateTimeKind.Local).AddTicks(1947), new DateTime(2020, 11, 26, 3, 1, 39, 544, DateTimeKind.Local).AddTicks(7615) });

            migrationBuilder.UpdateData(
                table: "NationalParks",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Created", "Established" },
                values: new object[] { new DateTime(2020, 11, 26, 3, 1, 39, 544, DateTimeKind.Local).AddTicks(8542), new DateTime(2020, 11, 26, 3, 1, 39, 544, DateTimeKind.Local).AddTicks(8567) });

            migrationBuilder.CreateIndex(
                name: "IX_Trails_NationalParkId",
                table: "Trails",
                column: "NationalParkId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Trails");

            migrationBuilder.UpdateData(
                table: "NationalParks",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Created", "Established" },
                values: new object[] { new DateTime(2020, 11, 24, 18, 6, 11, 619, DateTimeKind.Local).AddTicks(4112), new DateTime(2020, 11, 24, 18, 6, 11, 624, DateTimeKind.Local).AddTicks(6164) });

            migrationBuilder.UpdateData(
                table: "NationalParks",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Created", "Established" },
                values: new object[] { new DateTime(2020, 11, 24, 18, 6, 11, 624, DateTimeKind.Local).AddTicks(7083), new DateTime(2020, 11, 24, 18, 6, 11, 624, DateTimeKind.Local).AddTicks(7122) });
        }
    }
}
