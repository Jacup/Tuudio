using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tuudio.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Add_PassTemplatesV5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "PassTemplate",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0010-000000000000"),
                columns: new[] { "Duration_Amount", "Duration_Period", "Price_Amount", "Price_Period" },
                values: new object[] { 3, 3, 100.0m, 3 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
