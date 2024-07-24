using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tuudio.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Add_PassTemplatesV2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ActivityPassTemplate");

            migrationBuilder.DropColumn(
                name: "Duration_Amount",
                table: "PassTemplate");

            migrationBuilder.DropColumn(
                name: "Duration_Period",
                table: "PassTemplate");

            migrationBuilder.InsertData(
                table: "PassTemplate",
                columns: new[] { "Id", "CreatedDate", "Description", "Entries", "Name", "UpdatedDate", "Price_Period", "Price_PriceAmount" },
                values: new object[] { new Guid("00000000-0000-0000-0010-000000000000"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Pass for yoga classes, unlimited entries, paid monthly", 0, "Monthly yoga pass", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, 100.00m });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "PassTemplate",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0010-000000000000"));

            migrationBuilder.AddColumn<int>(
                name: "Duration_Amount",
                table: "PassTemplate",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Duration_Period",
                table: "PassTemplate",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "ActivityPassTemplate",
                columns: table => new
                {
                    ActivitiesId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    PassTemplatesId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActivityPassTemplate", x => new { x.ActivitiesId, x.PassTemplatesId });
                    table.ForeignKey(
                        name: "FK_ActivityPassTemplate_Activities_ActivitiesId",
                        column: x => x.ActivitiesId,
                        principalTable: "Activities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ActivityPassTemplate_PassTemplate_PassTemplatesId",
                        column: x => x.PassTemplatesId,
                        principalTable: "PassTemplate",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_ActivityPassTemplate_PassTemplatesId",
                table: "ActivityPassTemplate",
                column: "PassTemplatesId");
        }
    }
}
