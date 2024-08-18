using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tuudio.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class entriesv4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Entries",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0004-000000000001"),
                column: "EntryDate",
                value: new DateTime(2024, 8, 18, 19, 37, 4, 379, DateTimeKind.Local).AddTicks(3141));

            migrationBuilder.UpdateData(
                table: "Entries",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0004-000000000002"),
                column: "EntryDate",
                value: new DateTime(2024, 8, 18, 19, 37, 4, 379, DateTimeKind.Local).AddTicks(3188));

            migrationBuilder.UpdateData(
                table: "Passes",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0003-000000000001"),
                columns: new[] { "FromDate", "ToDate" },
                values: new object[] { new DateOnly(2024, 8, 18), new DateOnly(2024, 11, 18) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Entries",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0004-000000000001"),
                column: "EntryDate",
                value: new DateTime(2024, 8, 15, 22, 55, 1, 228, DateTimeKind.Local).AddTicks(2242));

            migrationBuilder.UpdateData(
                table: "Entries",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0004-000000000002"),
                column: "EntryDate",
                value: new DateTime(2024, 8, 15, 22, 55, 1, 228, DateTimeKind.Local).AddTicks(2290));

            migrationBuilder.UpdateData(
                table: "Passes",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0003-000000000001"),
                columns: new[] { "FromDate", "ToDate" },
                values: new object[] { new DateOnly(2024, 8, 15), new DateOnly(2024, 11, 15) });
        }
    }
}
