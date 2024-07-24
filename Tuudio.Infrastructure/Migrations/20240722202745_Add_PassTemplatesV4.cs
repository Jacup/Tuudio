using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tuudio.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Add_PassTemplatesV4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Price_PriceAmount",
                table: "PassTemplate",
                newName: "Price_Amount");

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

            migrationBuilder.UpdateData(
                table: "PassTemplate",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0010-000000000000"),
                column: "Description",
                value: "Pass for yoga classes, unlimited entries, paid monthly, 3 months");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Duration_Amount",
                table: "PassTemplate");

            migrationBuilder.DropColumn(
                name: "Duration_Period",
                table: "PassTemplate");

            migrationBuilder.RenameColumn(
                name: "Price_Amount",
                table: "PassTemplate",
                newName: "Price_PriceAmount");

            migrationBuilder.UpdateData(
                table: "PassTemplate",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0010-000000000000"),
                columns: new[] { "Description", "Price_Period", "Price_PriceAmount" },
                values: new object[] { "Pass for yoga classes, unlimited entries, paid monthly", 3, 100.00m });
        }
    }
}
