using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnboardingPlatform.Data.Migrations
{
    /// <inheritdoc />
    public partial class StockbrokingFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BrokerageFirm",
                table: "StockBrokingAccountDetails");

            migrationBuilder.DropColumn(
                name: "CscsNumber",
                table: "StockBrokingAccountDetails");

            migrationBuilder.DropColumn(
                name: "TradingAccountNumber",
                table: "StockBrokingAccountDetails");

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
                column: "CreatedAt",
                value: new DateTime(2026, 9, 17, 8, 47, 50, 448, DateTimeKind.Local).AddTicks(6984));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: new Guid("aaaaaaaa-0004-0004-0004-aaaaaaaaaaaa"),
                column: "CreatedAt",
                value: new DateTime(2026, 9, 17, 8, 47, 50, 448, DateTimeKind.Local).AddTicks(6989));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: new Guid("aaaaaaaa-0005-0005-0005-aaaaaaaaaaaa"),
                column: "CreatedAt",
                value: new DateTime(2026, 9, 17, 8, 47, 50, 448, DateTimeKind.Local).AddTicks(6994));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BrokerageFirm",
                table: "StockBrokingAccountDetails",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CscsNumber",
                table: "StockBrokingAccountDetails",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TradingAccountNumber",
                table: "StockBrokingAccountDetails",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: new Guid("aaaaaaaa-0001-0001-0001-aaaaaaaaaaaa"),
                column: "CreatedAt",
                value: new DateTime(2026, 9, 15, 20, 44, 19, 154, DateTimeKind.Local).AddTicks(2240));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: new Guid("aaaaaaaa-0002-0002-0002-aaaaaaaaaaaa"),
                column: "CreatedAt",
                value: new DateTime(2026, 9, 15, 20, 44, 19, 154, DateTimeKind.Local).AddTicks(2252));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: new Guid("aaaaaaaa-0003-0003-0003-aaaaaaaaaaaa"),
                column: "CreatedAt",
                value: new DateTime(2026, 9, 15, 20, 44, 19, 154, DateTimeKind.Local).AddTicks(2261));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: new Guid("aaaaaaaa-0004-0004-0004-aaaaaaaaaaaa"),
                column: "CreatedAt",
                value: new DateTime(2026, 9, 15, 20, 44, 19, 154, DateTimeKind.Local).AddTicks(2278));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: new Guid("aaaaaaaa-0005-0005-0005-aaaaaaaaaaaa"),
                column: "CreatedAt",
                value: new DateTime(2026, 9, 15, 20, 44, 19, 154, DateTimeKind.Local).AddTicks(2296));
        }
    }
}
