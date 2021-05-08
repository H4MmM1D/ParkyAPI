using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ParkyAPI.Migrations
{
    public partial class SeedingNationalParkData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "NationalParks",
                columns: new[] { "Id", "Created", "Established", "Name", "Picture", "State" },
                values: new object[] { 1, new DateTime(2020, 11, 24, 18, 6, 11, 619, DateTimeKind.Local).AddTicks(4112), new DateTime(2020, 11, 24, 18, 6, 11, 624, DateTimeKind.Local).AddTicks(6164), "NP", null, "IL" });

            migrationBuilder.InsertData(
                table: "NationalParks",
                columns: new[] { "Id", "Created", "Established", "Name", "Picture", "State" },
                values: new object[] { 2, new DateTime(2020, 11, 24, 18, 6, 11, 624, DateTimeKind.Local).AddTicks(7083), new DateTime(2020, 11, 24, 18, 6, 11, 624, DateTimeKind.Local).AddTicks(7122), "NPTest", null, "TS" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "NationalParks",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "NationalParks",
                keyColumn: "Id",
                keyValue: 2);
        }
    }
}
