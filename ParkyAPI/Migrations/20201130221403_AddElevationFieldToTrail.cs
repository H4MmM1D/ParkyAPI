using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ParkyAPI.Migrations
{
    public partial class AddElevationFieldToTrail : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Elevation",
                table: "Trails",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Elevation",
                table: "Trails");

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
        }
    }
}
