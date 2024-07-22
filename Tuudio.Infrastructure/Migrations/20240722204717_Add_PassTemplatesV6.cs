using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tuudio.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Add_PassTemplatesV6 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.InsertData(
                table: "ActivityPassTemplate",
                columns: new[] { "ActivitiesId", "PassTemplatesId" },
                values: new object[] { new Guid("00000000-0000-0000-0001-000000000001"), new Guid("00000000-0000-0000-0010-000000000000") });

            migrationBuilder.CreateIndex(
                name: "IX_ActivityPassTemplate_PassTemplatesId",
                table: "ActivityPassTemplate",
                column: "PassTemplatesId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ActivityPassTemplate");
        }
    }
}
