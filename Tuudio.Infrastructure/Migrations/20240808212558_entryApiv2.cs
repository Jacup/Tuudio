using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tuudio.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class entryApiv2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Entry_Clients_ClientId",
                table: "Entry");

            migrationBuilder.DropForeignKey(
                name: "FK_Entry_Passes_PassId",
                table: "Entry");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Entry",
                table: "Entry");

            migrationBuilder.RenameTable(
                name: "Entry",
                newName: "Entries");

            migrationBuilder.RenameIndex(
                name: "IX_Entry_PassId",
                table: "Entries",
                newName: "IX_Entries_PassId");

            migrationBuilder.RenameIndex(
                name: "IX_Entry_ClientId",
                table: "Entries",
                newName: "IX_Entries_ClientId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Entries",
                table: "Entries",
                column: "Id");

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

            migrationBuilder.AddForeignKey(
                name: "FK_Entries_Clients_ClientId",
                table: "Entries",
                column: "ClientId",
                principalTable: "Clients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Entries_Passes_PassId",
                table: "Entries",
                column: "PassId",
                principalTable: "Passes",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Entries_Clients_ClientId",
                table: "Entries");

            migrationBuilder.DropForeignKey(
                name: "FK_Entries_Passes_PassId",
                table: "Entries");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Entries",
                table: "Entries");

            migrationBuilder.RenameTable(
                name: "Entries",
                newName: "Entry");

            migrationBuilder.RenameIndex(
                name: "IX_Entries_PassId",
                table: "Entry",
                newName: "IX_Entry_PassId");

            migrationBuilder.RenameIndex(
                name: "IX_Entries_ClientId",
                table: "Entry",
                newName: "IX_Entry_ClientId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Entry",
                table: "Entry",
                column: "Id");

            migrationBuilder.UpdateData(
                table: "Entry",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0004-000000000001"),
                column: "EntryDate",
                value: new DateTime(2024, 8, 8, 23, 19, 34, 130, DateTimeKind.Local).AddTicks(700));

            migrationBuilder.UpdateData(
                table: "Entry",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0004-000000000002"),
                column: "EntryDate",
                value: new DateTime(2024, 8, 8, 23, 19, 34, 130, DateTimeKind.Local).AddTicks(746));

            migrationBuilder.AddForeignKey(
                name: "FK_Entry_Clients_ClientId",
                table: "Entry",
                column: "ClientId",
                principalTable: "Clients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Entry_Passes_PassId",
                table: "Entry",
                column: "PassId",
                principalTable: "Passes",
                principalColumn: "Id");
        }
    }
}
