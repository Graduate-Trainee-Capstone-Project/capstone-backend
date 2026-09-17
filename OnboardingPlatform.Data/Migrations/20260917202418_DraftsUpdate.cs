using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnboardingPlatform.Data.Migrations
{
    /// <inheritdoc />
    public partial class DraftsUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: new Guid("aaaaaaaa-0001-0001-0001-aaaaaaaaaaaa"),
                column: "CreatedAt",
                value: new DateTime(2026, 9, 17, 21, 24, 16, 932, DateTimeKind.Local).AddTicks(351));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: new Guid("aaaaaaaa-0002-0002-0002-aaaaaaaaaaaa"),
                column: "CreatedAt",
                value: new DateTime(2026, 9, 17, 21, 24, 16, 932, DateTimeKind.Local).AddTicks(362));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: new Guid("aaaaaaaa-0003-0003-0003-aaaaaaaaaaaa"),
                columns: new[] { "CreatedAt", "RequiredIdentifiers" },
                values: new object[] { new DateTime(2026, 9, 17, 21, 24, 16, 932, DateTimeKind.Local).AddTicks(370), "[\"BVN\",\"NIN\"]" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: new Guid("aaaaaaaa-0004-0004-0004-aaaaaaaaaaaa"),
                columns: new[] { "CreatedAt", "RequiredIdentifiers" },
                values: new object[] { new DateTime(2026, 9, 17, 21, 24, 16, 932, DateTimeKind.Local).AddTicks(378), "[\"BVN\",\"EMAIL\"]" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: new Guid("aaaaaaaa-0005-0005-0005-aaaaaaaaaaaa"),
                columns: new[] { "CreatedAt", "RequiredIdentifiers" },
                values: new object[] { new DateTime(2026, 9, 17, 21, 24, 16, 932, DateTimeKind.Local).AddTicks(385), "[\"BVN\",\"PHONE\"]" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: new Guid("aaaaaaaa-0001-0001-0001-aaaaaaaaaaaa"),
                column: "CreatedAt",
                value: new DateTime(2026, 9, 17, 8, 47, 50, 448, DateTimeKind.Local).AddTicks(6957));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: new Guid("aaaaaaaa-0002-0002-0002-aaaaaaaaaaaa"),
                column: "CreatedAt",
                value: new DateTime(2026, 9, 17, 8, 47, 50, 448, DateTimeKind.Local).AddTicks(6978));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: new Guid("aaaaaaaa-0003-0003-0003-aaaaaaaaaaaa"),
                columns: new[] { "CreatedAt", "RequiredIdentifiers" },
                values: new object[] { new DateTime(2026, 9, 17, 8, 47, 50, 448, DateTimeKind.Local).AddTicks(6984), "[\"NIN\",\"PHONE\"]" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: new Guid("aaaaaaaa-0004-0004-0004-aaaaaaaaaaaa"),
                columns: new[] { "CreatedAt", "RequiredIdentifiers" },
                values: new object[] { new DateTime(2026, 9, 17, 8, 47, 50, 448, DateTimeKind.Local).AddTicks(6989), "[\"EMAIL\"]" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: new Guid("aaaaaaaa-0005-0005-0005-aaaaaaaaaaaa"),
                columns: new[] { "CreatedAt", "RequiredIdentifiers" },
                values: new object[] { new DateTime(2026, 9, 17, 8, 47, 50, 448, DateTimeKind.Local).AddTicks(6994), "[\"EMAIL\",\"PHONE\"]" });
        }
    }
}
