using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tuudio.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Add_PassClient2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ActivityPassTemplate",
                keyColumns: new[] { "ActivitiesId", "PassTemplatesId" },
                keyValues: new object[] { new Guid("00000000-0000-0000-0001-000000000001"), new Guid("00000000-0000-0000-0010-000000000000") });

            migrationBuilder.DeleteData(
                table: "PassTemplate",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0010-000000000000"));

            migrationBuilder.InsertData(
                table: "PassTemplate",
                columns: new[] { "Id", "Duration_Amount", "Duration_Period", "CreatedDate", "Description", "Entries", "Name", "UpdatedDate", "Price_Amount", "Price_Period" },
                values: new object[] { new Guid("00000000-0000-0000-0002-000000000001"), 3, 3, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Pass for yoga classes, unlimited entries, paid monthly, 3 months", 0, "Monthly yoga pass", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 100.0m, 3 });

            migrationBuilder.InsertData(
                table: "ActivityPassTemplate",
                columns: new[] { "ActivitiesId", "PassTemplatesId" },
                values: new object[] { new Guid("00000000-0000-0000-0001-000000000001"), new Guid("00000000-0000-0000-0002-000000000001") });

            migrationBuilder.InsertData(
                table: "Pass",
                columns: new[] { "Id", "ClientId", "CreatedDate", "FromDate", "PassTemplateId", "ToDate", "UpdatedDate" },
                values: new object[] { new Guid("00000000-0000-0000-0003-000000000001"), new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateOnly(2024, 7, 28), new Guid("00000000-0000-0000-0002-000000000001"), new DateOnly(2024, 10, 28), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ActivityPassTemplate",
                keyColumns: new[] { "ActivitiesId", "PassTemplatesId" },
                keyValues: new object[] { new Guid("00000000-0000-0000-0001-000000000001"), new Guid("00000000-0000-0000-0002-000000000001") });

            migrationBuilder.DeleteData(
                table: "Pass",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0003-000000000001"));

            migrationBuilder.DeleteData(
                table: "PassTemplate",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0002-000000000001"));

            migrationBuilder.InsertData(
                table: "PassTemplate",
                columns: new[] { "Id", "CreatedDate", "Description", "Entries", "Name", "UpdatedDate", "Duration_Amount", "Duration_Period", "Price_Amount", "Price_Period" },
                values: new object[] { new Guid("00000000-0000-0000-0010-000000000000"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Pass for yoga classes, unlimited entries, paid monthly, 3 months", 0, "Monthly yoga pass", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, 3, 100.0m, 3 });

            migrationBuilder.InsertData(
                table: "ActivityPassTemplate",
                columns: new[] { "ActivitiesId", "PassTemplatesId" },
                values: new object[] { new Guid("00000000-0000-0000-0001-000000000001"), new Guid("00000000-0000-0000-0010-000000000000") });
        }
    }
}
