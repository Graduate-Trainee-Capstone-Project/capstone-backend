using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnboardingPlatform.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddCustomerEmailandPhoneFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Customers",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "Customers",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "Customers");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: new Guid("aaaaaaaa-0001-0001-0001-aaaaaaaaaaaa"),
                column: "CreatedAt",
                value: new DateTime(2026, 9, 14, 19, 4, 29, 713, DateTimeKind.Local).AddTicks(9623));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: new Guid("aaaaaaaa-0002-0002-0002-aaaaaaaaaaaa"),
                column: "CreatedAt",
                value: new DateTime(2026, 9, 14, 19, 4, 29, 713, DateTimeKind.Local).AddTicks(9638));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: new Guid("aaaaaaaa-0003-0003-0003-aaaaaaaaaaaa"),
                column: "CreatedAt",
                value: new DateTime(2026, 9, 14, 19, 4, 29, 713, DateTimeKind.Local).AddTicks(9643));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: new Guid("aaaaaaaa-0004-0004-0004-aaaaaaaaaaaa"),
                column: "CreatedAt",
                value: new DateTime(2026, 9, 14, 19, 4, 29, 713, DateTimeKind.Local).AddTicks(9646));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: new Guid("aaaaaaaa-0005-0005-0005-aaaaaaaaaaaa"),
                column: "CreatedAt",
                value: new DateTime(2026, 9, 14, 19, 4, 29, 713, DateTimeKind.Local).AddTicks(9649));
        }
    }
}
