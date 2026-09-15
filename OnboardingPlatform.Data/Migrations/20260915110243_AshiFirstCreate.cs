using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnboardingPlatform.Data.Migrations
{
    /// <inheritdoc />
    public partial class AshiFirstCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ConsentLogs",
                columns: table => new
                {
                    ConsentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CustomerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ConsentType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GrantedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Channel = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConsentLogs", x => x.ConsentId);
                    table.ForeignKey(
                        name: "FK_ConsentLogs_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "CustomerId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ConsentLogs_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CurrentAccountDetails",
                columns: table => new
                {
                    CurrentAccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CustomerProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AccountNumber = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Currency = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    Balance = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CheckBookRequested = table.Column<bool>(type: "bit", nullable: false),
                    DateOpened = table.Column<DateOnly>(type: "date", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CurrentAccountDetails", x => x.CurrentAccountId);
                    table.ForeignKey(
                        name: "FK_CurrentAccountDetails_CustomerProducts_CustomerProductId",
                        column: x => x.CustomerProductId,
                        principalTable: "CustomerProducts",
                        principalColumn: "CustomerProductId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PensionAccountDetails",
                columns: table => new
                {
                    PensionAccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CustomerProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RsaPin = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PfaName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EmployerName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContributionScheme = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateRegistered = table.Column<DateOnly>(type: "date", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PensionAccountDetails", x => x.PensionAccountId);
                    table.ForeignKey(
                        name: "FK_PensionAccountDetails_CustomerProducts_CustomerProductId",
                        column: x => x.CustomerProductId,
                        principalTable: "CustomerProducts",
                        principalColumn: "CustomerProductId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SavingsAccountDetails",
                columns: table => new
                {
                    SavingsAccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CustomerProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AccountNumber = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Currency = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    Balance = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DateOpened = table.Column<DateOnly>(type: "date", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SavingsAccountDetails", x => x.SavingsAccountId);
                    table.ForeignKey(
                        name: "FK_SavingsAccountDetails_CustomerProducts_CustomerProductId",
                        column: x => x.CustomerProductId,
                        principalTable: "CustomerProducts",
                        principalColumn: "CustomerProductId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SecurityChecks",
                columns: table => new
                {
                    SecurityCheckId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DraftId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CheckType = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CompletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SecurityChecks", x => x.SecurityCheckId);
                    table.ForeignKey(
                        name: "FK_SecurityChecks_DraftApplications_DraftId",
                        column: x => x.DraftId,
                        principalTable: "DraftApplications",
                        principalColumn: "DraftId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: new Guid("aaaaaaaa-0001-0001-0001-aaaaaaaaaaaa"),
                column: "CreatedAt",
                value: new DateTime(2026, 9, 15, 12, 2, 42, 846, DateTimeKind.Local).AddTicks(6642));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: new Guid("aaaaaaaa-0002-0002-0002-aaaaaaaaaaaa"),
                column: "CreatedAt",
                value: new DateTime(2026, 9, 15, 12, 2, 42, 846, DateTimeKind.Local).AddTicks(6661));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: new Guid("aaaaaaaa-0003-0003-0003-aaaaaaaaaaaa"),
                column: "CreatedAt",
                value: new DateTime(2026, 9, 15, 12, 2, 42, 846, DateTimeKind.Local).AddTicks(6665));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: new Guid("aaaaaaaa-0004-0004-0004-aaaaaaaaaaaa"),
                column: "CreatedAt",
                value: new DateTime(2026, 9, 15, 12, 2, 42, 846, DateTimeKind.Local).AddTicks(6668));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: new Guid("aaaaaaaa-0005-0005-0005-aaaaaaaaaaaa"),
                column: "CreatedAt",
                value: new DateTime(2026, 9, 15, 12, 2, 42, 846, DateTimeKind.Local).AddTicks(6672));

            migrationBuilder.CreateIndex(
                name: "IX_ConsentLogs_CustomerId",
                table: "ConsentLogs",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_ConsentLogs_ProductId",
                table: "ConsentLogs",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_CurrentAccountDetails_CustomerProductId",
                table: "CurrentAccountDetails",
                column: "CustomerProductId");

            migrationBuilder.CreateIndex(
                name: "IX_PensionAccountDetails_CustomerProductId",
                table: "PensionAccountDetails",
                column: "CustomerProductId");

            migrationBuilder.CreateIndex(
                name: "IX_SavingsAccountDetails_CustomerProductId",
                table: "SavingsAccountDetails",
                column: "CustomerProductId");

            migrationBuilder.CreateIndex(
                name: "IX_SecurityChecks_DraftId",
                table: "SecurityChecks",
                column: "DraftId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ConsentLogs");

            migrationBuilder.DropTable(
                name: "CurrentAccountDetails");

            migrationBuilder.DropTable(
                name: "PensionAccountDetails");

            migrationBuilder.DropTable(
                name: "SavingsAccountDetails");

            migrationBuilder.DropTable(
                name: "SecurityChecks");

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
