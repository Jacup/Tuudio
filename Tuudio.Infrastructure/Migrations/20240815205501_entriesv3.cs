using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tuudio.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class entriesv3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Entries",
                table: "PassTemplate",
                newName: "EntriesAmount");

            migrationBuilder.AddColumn<Guid>(
                name: "ActivityId",
                table: "Entries",
                type: "char(36)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                collation: "ascii_general_ci");

            migrationBuilder.UpdateData(
                table: "Entries",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0004-000000000001"),
                columns: new[] { "ActivityId", "EntryDate" },
                values: new object[] { new Guid("00000000-0000-0000-0001-000000000001"), new DateTime(2024, 8, 15, 22, 55, 1, 228, DateTimeKind.Local).AddTicks(2242) });

            migrationBuilder.UpdateData(
                table: "Entries",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0004-000000000002"),
                columns: new[] { "ActivityId", "EntryDate" },
                values: new object[] { new Guid("00000000-0000-0000-0001-000000000001"), new DateTime(2024, 8, 15, 22, 55, 1, 228, DateTimeKind.Local).AddTicks(2290) });

            migrationBuilder.UpdateData(
                table: "Passes",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0003-000000000001"),
                columns: new[] { "FromDate", "ToDate" },
                values: new object[] { new DateOnly(2024, 8, 15), new DateOnly(2024, 11, 15) });

            migrationBuilder.CreateIndex(
                name: "IX_Entries_ActivityId",
                table: "Entries",
                column: "ActivityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Entries_Activities_ActivityId",
                table: "Entries",
                column: "ActivityId",
                principalTable: "Activities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Entries_Activities_ActivityId",
                table: "Entries");

            migrationBuilder.DropIndex(
                name: "IX_Entries_ActivityId",
                table: "Entries");

            migrationBuilder.DropColumn(
                name: "ActivityId",
                table: "Entries");

            migrationBuilder.RenameColumn(
                name: "EntriesAmount",
                table: "PassTemplate",
                newName: "Entries");

            migrationBuilder.UpdateData(
                table: "Entries",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0004-000000000001"),
                column: "EntryDate",
                value: new DateTime(2024, 8, 8, 23, 25, 57, 718, DateTimeKind.Local).AddTicks(3330));

            migrationBuilder.UpdateData(
                table: "Entries",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0004-000000000002"),
                column: "EntryDate",
                value: new DateTime(2024, 8, 8, 23, 25, 57, 718, DateTimeKind.Local).AddTicks(3378));

            migrationBuilder.UpdateData(
                table: "Passes",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0003-000000000001"),
                columns: new[] { "FromDate", "ToDate" },
                values: new object[] { new DateOnly(2024, 8, 8), new DateOnly(2024, 11, 8) });
        }
    }
}
