using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace OnboardingPlatform.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    CustomerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MiddleName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Gender = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Nationality = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.CustomerId);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    ProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProductCode = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProductName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RequiredIdentifiers = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AdditionalFieldsSchema = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.ProductId);
                });

            migrationBuilder.CreateTable(
                name: "CustomerAddresses",
                columns: table => new
                {
                    AddressId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CustomerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    HouseNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Street = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    State = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Country = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsPrimary = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerAddresses", x => x.AddressId);
                    table.ForeignKey(
                        name: "FK_CustomerAddresses_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "CustomerId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CustomerIdentifiers",
                columns: table => new
                {
                    IdentifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CustomerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IdentifierType = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IdentifierValueHash = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IdentifierValueMasked = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsVerified = table.Column<bool>(type: "bit", nullable: false),
                    VerifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerIdentifiers", x => x.IdentifierId);
                    table.ForeignKey(
                        name: "FK_CustomerIdentifiers_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "CustomerId",
                        onDelete: ReferentialAction.Cascade);
                });

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
                name: "CustomerProducts",
                columns: table => new
                {
                    CustomerProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CustomerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DraftId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerProducts", x => x.CustomerProductId);
                    table.ForeignKey(
                        name: "FK_CustomerProducts_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "CustomerId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CustomerProducts_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DraftApplications",
                columns: table => new
                {
                    DraftId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CustomerId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    PrimaryIdentifierType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PrimaryIdentifierValueHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SecondaryIdentifierType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecondaryIdentifierValueHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CurrentStep = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FormDataJson = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Channel = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastUpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExpiresAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DraftApplications", x => x.DraftId);
                    table.ForeignKey(
                        name: "FK_DraftApplications_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "CustomerId");
                    table.ForeignKey(
                        name: "FK_DraftApplications_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Cascade);
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
                name: "StockBrokingAccountDetails",
                columns: table => new
                {
                    StockBrokingAccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CustomerProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CscsNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    BrokerageFirm = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TradingAccountNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    DateOpened = table.Column<DateOnly>(type: "date", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockBrokingAccountDetails", x => x.StockBrokingAccountId);
                    table.ForeignKey(
                        name: "FK_StockBrokingAccountDetails_CustomerProducts_CustomerProductId",
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

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "ProductId", "AdditionalFieldsSchema", "CreatedAt", "IsActive", "ProductCode", "ProductName", "RequiredIdentifiers" },
                values: new object[,]
                {
                    { new Guid("aaaaaaaa-0001-0001-0001-aaaaaaaaaaaa"), "[\r\n  {\r\n    \"field\": \"branchPreference\",\r\n    \"label\": \"Preferred branch\",\r\n    \"type\": \"text\",\r\n    \"required\": false\r\n  }\r\n]", new DateTime(2026, 9, 15, 20, 44, 19, 154, DateTimeKind.Local).AddTicks(2240), true, "SAVINGS", "Savings Account", "[\"BVN\"]" },
                    { new Guid("aaaaaaaa-0002-0002-0002-aaaaaaaaaaaa"), null, new DateTime(2026, 9, 15, 20, 44, 19, 154, DateTimeKind.Local).AddTicks(2252), true, "CURRENT", "Current Account", "[\"BVN\"]" },
                    { new Guid("aaaaaaaa-0003-0003-0003-aaaaaaaaaaaa"), "[\r\n  {\r\n    \"field\": \"employerName\",\r\n    \"label\": \"Employer name\",\r\n    \"type\": \"text\",\r\n    \"required\": false\r\n  },\r\n  {\r\n    \"field\": \"contributionScheme\",\r\n    \"label\": \"Contribution scheme\",\r\n    \"type\": \"select\",\r\n    \"options\": [\r\n      \"MandatoryCPS\",\r\n      \"Voluntary\",\r\n      \"MicroPensionPlan\"\r\n    ],\r\n    \"required\": true\r\n  }\r\n]", new DateTime(2026, 9, 15, 20, 44, 19, 154, DateTimeKind.Local).AddTicks(2261), true, "PENSION_RSA", "Pension (RSA)", "[\"NIN\",\"PHONE\"]" },
                    { new Guid("aaaaaaaa-0004-0004-0004-aaaaaaaaaaaa"), null, new DateTime(2026, 9, 15, 20, 44, 19, 154, DateTimeKind.Local).AddTicks(2278), true, "STOCKBROKING", "Stockbroking", "[\"EMAIL\"]" },
                    { new Guid("aaaaaaaa-0005-0005-0005-aaaaaaaaaaaa"), null, new DateTime(2026, 9, 15, 20, 44, 19, 154, DateTimeKind.Local).AddTicks(2296), true, "INSURANCE", "Insurance", "[\"EMAIL\",\"PHONE\"]" }
                });

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
                name: "IX_CustomerAddresses_CustomerId",
                table: "CustomerAddresses",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerIdentifiers_CustomerId",
                table: "CustomerIdentifiers",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerIdentifiers_IdentifierType_IdentifierValueHash",
                table: "CustomerIdentifiers",
                columns: new[] { "IdentifierType", "IdentifierValueHash" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CustomerProducts_CustomerId_ProductId",
                table: "CustomerProducts",
                columns: new[] { "CustomerId", "ProductId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CustomerProducts_ProductId",
                table: "CustomerProducts",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_DraftApplications_CustomerId",
                table: "DraftApplications",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_DraftApplications_ProductId",
                table: "DraftApplications",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_PensionAccountDetails_CustomerProductId",
                table: "PensionAccountDetails",
                column: "CustomerProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_ProductCode",
                table: "Products",
                column: "ProductCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SavingsAccountDetails_CustomerProductId",
                table: "SavingsAccountDetails",
                column: "CustomerProductId");

            migrationBuilder.CreateIndex(
                name: "IX_SecurityChecks_DraftId",
                table: "SecurityChecks",
                column: "DraftId");

            migrationBuilder.CreateIndex(
                name: "IX_StockBrokingAccountDetails_CustomerProductId",
                table: "StockBrokingAccountDetails",
                column: "CustomerProductId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ConsentLogs");

            migrationBuilder.DropTable(
                name: "CurrentAccountDetails");

            migrationBuilder.DropTable(
                name: "CustomerAddresses");

            migrationBuilder.DropTable(
                name: "CustomerIdentifiers");

            migrationBuilder.DropTable(
                name: "PensionAccountDetails");

            migrationBuilder.DropTable(
                name: "SavingsAccountDetails");

            migrationBuilder.DropTable(
                name: "SecurityChecks");

            migrationBuilder.DropTable(
                name: "StockBrokingAccountDetails");

            migrationBuilder.DropTable(
                name: "DraftApplications");

            migrationBuilder.DropTable(
                name: "CustomerProducts");

            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.DropTable(
                name: "Products");
        }
    }
}
