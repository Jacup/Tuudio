using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tuudio.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class clientRename : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Entries_Clients_ClientId",
                table: "Entries");

            migrationBuilder.DropForeignKey(
                name: "FK_Passes_Clients_ClientId",
                table: "Passes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Clients",
                table: "Clients");

            migrationBuilder.RenameTable(
                name: "Clients",
                newName: "Client");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Client",
                table: "Client",
                column: "Id");

            migrationBuilder.UpdateData(
                table: "Entries",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0004-000000000001"),
                column: "EntryDate",
                value: new DateTime(2024, 8, 18, 23, 10, 8, 57, DateTimeKind.Local).AddTicks(6455));

            migrationBuilder.UpdateData(
                table: "Entries",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0004-000000000002"),
                column: "EntryDate",
                value: new DateTime(2024, 8, 18, 23, 10, 8, 57, DateTimeKind.Local).AddTicks(6503));

            migrationBuilder.AddForeignKey(
                name: "FK_Entries_Client_ClientId",
                table: "Entries",
                column: "ClientId",
                principalTable: "Client",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Passes_Client_ClientId",
                table: "Passes",
                column: "ClientId",
                principalTable: "Client",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Entries_Client_ClientId",
                table: "Entries");

            migrationBuilder.DropForeignKey(
                name: "FK_Passes_Client_ClientId",
                table: "Passes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Client",
                table: "Client");

            migrationBuilder.RenameTable(
                name: "Client",
                newName: "Clients");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Clients",
                table: "Clients",
                column: "Id");

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

            migrationBuilder.AddForeignKey(
                name: "FK_Entries_Clients_ClientId",
                table: "Entries",
                column: "ClientId",
                principalTable: "Clients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Passes_Clients_ClientId",
                table: "Passes",
                column: "ClientId",
                principalTable: "Clients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
