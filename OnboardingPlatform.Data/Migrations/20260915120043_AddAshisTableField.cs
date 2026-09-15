using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnboardingPlatform.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddAshisTableField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: new Guid("aaaaaaaa-0001-0001-0001-aaaaaaaaaaaa"),
                column: "CreatedAt",
                value: new DateTime(2026, 9, 15, 13, 0, 41, 548, DateTimeKind.Local).AddTicks(7987));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: new Guid("aaaaaaaa-0002-0002-0002-aaaaaaaaaaaa"),
                column: "CreatedAt",
                value: new DateTime(2026, 9, 15, 13, 0, 41, 548, DateTimeKind.Local).AddTicks(7995));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: new Guid("aaaaaaaa-0003-0003-0003-aaaaaaaaaaaa"),
                column: "CreatedAt",
                value: new DateTime(2026, 9, 15, 13, 0, 41, 548, DateTimeKind.Local).AddTicks(8002));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: new Guid("aaaaaaaa-0004-0004-0004-aaaaaaaaaaaa"),
                column: "CreatedAt",
                value: new DateTime(2026, 9, 15, 13, 0, 41, 548, DateTimeKind.Local).AddTicks(8025));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: new Guid("aaaaaaaa-0005-0005-0005-aaaaaaaaaaaa"),
                column: "CreatedAt",
                value: new DateTime(2026, 9, 15, 13, 0, 41, 548, DateTimeKind.Local).AddTicks(8032));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: new Guid("aaaaaaaa-0001-0001-0001-aaaaaaaaaaaa"),
                column: "CreatedAt",
                value: new DateTime(2026, 9, 15, 6, 29, 54, 906, DateTimeKind.Local).AddTicks(9910));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: new Guid("aaaaaaaa-0002-0002-0002-aaaaaaaaaaaa"),
                column: "CreatedAt",
                value: new DateTime(2026, 9, 15, 6, 29, 54, 906, DateTimeKind.Local).AddTicks(9916));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: new Guid("aaaaaaaa-0003-0003-0003-aaaaaaaaaaaa"),
                column: "CreatedAt",
                value: new DateTime(2026, 9, 15, 6, 29, 54, 906, DateTimeKind.Local).AddTicks(9921));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: new Guid("aaaaaaaa-0004-0004-0004-aaaaaaaaaaaa"),
                column: "CreatedAt",
                value: new DateTime(2026, 9, 15, 6, 29, 54, 906, DateTimeKind.Local).AddTicks(9925));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: new Guid("aaaaaaaa-0005-0005-0005-aaaaaaaaaaaa"),
                column: "CreatedAt",
                value: new DateTime(2026, 9, 15, 6, 29, 54, 906, DateTimeKind.Local).AddTicks(9943));
        }
    }
}
