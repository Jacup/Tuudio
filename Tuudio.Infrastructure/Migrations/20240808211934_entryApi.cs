using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Tuudio.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class entryApi : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pass_Clients_ClientId",
                table: "Pass");

            migrationBuilder.DropForeignKey(
                name: "FK_Pass_PassTemplate_PassTemplateId",
                table: "Pass");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Pass",
                table: "Pass");

            migrationBuilder.RenameTable(
                name: "Pass",
                newName: "Passes");

            migrationBuilder.RenameIndex(
                name: "IX_Pass_PassTemplateId",
                table: "Passes",
                newName: "IX_Passes_PassTemplateId");

            migrationBuilder.RenameIndex(
                name: "IX_Pass_ClientId",
                table: "Passes",
                newName: "IX_Passes_ClientId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Passes",
                table: "Passes",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Entry",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    EntryDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Note = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ClientId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    PassId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    CreatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Entry", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Entry_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Entry_Passes_PassId",
                        column: x => x.PassId,
                        principalTable: "Passes",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.InsertData(
                table: "Entry",
                columns: new[] { "Id", "ClientId", "CreatedDate", "EntryDate", "Note", "PassId", "UpdatedDate" },
                values: new object[,]
                {
                    { new Guid("00000000-0000-0000-0004-000000000001"), new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 8, 8, 23, 19, 34, 130, DateTimeKind.Local).AddTicks(700), "Client first entry without pass", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("00000000-0000-0000-0004-000000000002"), new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 8, 8, 23, 19, 34, 130, DateTimeKind.Local).AddTicks(746), "Client second entry with pass", new Guid("00000000-0000-0000-0003-000000000001"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.UpdateData(
                table: "Passes",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0003-000000000001"),
                columns: new[] { "FromDate", "ToDate" },
                values: new object[] { new DateOnly(2024, 8, 8), new DateOnly(2024, 11, 8) });

            migrationBuilder.CreateIndex(
                name: "IX_Entry_ClientId",
                table: "Entry",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_Entry_PassId",
                table: "Entry",
                column: "PassId");

            migrationBuilder.AddForeignKey(
                name: "FK_Passes_Clients_ClientId",
                table: "Passes",
                column: "ClientId",
                principalTable: "Clients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Passes_PassTemplate_PassTemplateId",
                table: "Passes",
                column: "PassTemplateId",
                principalTable: "PassTemplate",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Passes_Clients_ClientId",
                table: "Passes");

            migrationBuilder.DropForeignKey(
                name: "FK_Passes_PassTemplate_PassTemplateId",
                table: "Passes");

            migrationBuilder.DropTable(
                name: "Entry");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Passes",
                table: "Passes");

            migrationBuilder.RenameTable(
                name: "Passes",
                newName: "Pass");

            migrationBuilder.RenameIndex(
                name: "IX_Passes_PassTemplateId",
                table: "Pass",
                newName: "IX_Pass_PassTemplateId");

            migrationBuilder.RenameIndex(
                name: "IX_Passes_ClientId",
                table: "Pass",
                newName: "IX_Pass_ClientId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Pass",
                table: "Pass",
                column: "Id");

            migrationBuilder.UpdateData(
                table: "Pass",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0003-000000000001"),
                columns: new[] { "FromDate", "ToDate" },
                values: new object[] { new DateOnly(2024, 7, 28), new DateOnly(2024, 10, 28) });

            migrationBuilder.AddForeignKey(
                name: "FK_Pass_Clients_ClientId",
                table: "Pass",
                column: "ClientId",
                principalTable: "Clients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Pass_PassTemplate_PassTemplateId",
                table: "Pass",
                column: "PassTemplateId",
                principalTable: "PassTemplate",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
