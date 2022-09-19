using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AccountHolders",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar", nullable: false),
                    Name = table.Column<string>(type: "varchar", maxLength: 50, nullable: false),
                    Surname = table.Column<string>(type: "varchar", maxLength: 50, nullable: false),
                    PhoneNumber = table.Column<string>(type: "TEXT", nullable: false),
                    BSN = table.Column<string>(type: "varchar", maxLength: 50, nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifyDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountHolders", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "IbanStores",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IBAN = table.Column<string>(type: "varchar", maxLength: 18, nullable: false),
                    AccountNumber = table.Column<string>(type: "varchar", maxLength: 10, nullable: false),
                    IsActive = table.Column<bool>(type: "byte", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IbanStores", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TransactionFees",
                columns: table => new
                {
                    Id = table.Column<string>(type: "int", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Percentage = table.Column<decimal>(type: "decimal", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifyDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransactionFees", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TransactionHistories",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    Type = table.Column<int>(type: "INTEGER", nullable: false),
                    AccountHolderId = table.Column<string>(type: "TEXT", nullable: false),
                    ReferenceNumber = table.Column<string>(type: "TEXT", nullable: false),
                    Data = table.Column<string>(type: "TEXT", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ModifyDate = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransactionHistories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TransactionLimits",
                columns: table => new
                {
                    Id = table.Column<string>(type: "int", nullable: false),
                    MaxAmount = table.Column<decimal>(type: "decimal", maxLength: 18, nullable: false),
                    MinAmount = table.Column<decimal>(type: "decimal", maxLength: 18, nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifyDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransactionLimits", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Accounts",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar", nullable: false),
                    HolderId = table.Column<string>(type: "int", nullable: false),
                    AccountNumber = table.Column<string>(type: "varchar", nullable: false),
                    IBAN = table.Column<string>(type: "varchar", maxLength: 50, nullable: false),
                    Balance = table.Column<decimal>(type: "decimal", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifyDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accounts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Accounts_AccountHolders_HolderId",
                        column: x => x.HolderId,
                        principalTable: "AccountHolders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "IbanStores",
                columns: new[] { "Id", "AccountNumber", "IBAN", "IsActive" },
                values: new object[] { 1, "8261521222", "NL21ABNA8261521222", false });

            migrationBuilder.InsertData(
                table: "IbanStores",
                columns: new[] { "Id", "AccountNumber", "IBAN", "IsActive" },
                values: new object[] { 2, "4883846911", "NL18RABO4883846911", false });

            migrationBuilder.InsertData(
                table: "IbanStores",
                columns: new[] { "Id", "AccountNumber", "IBAN", "IsActive" },
                values: new object[] { 3, "3767744449", "NL35ABNA3767744449", false });

            migrationBuilder.InsertData(
                table: "IbanStores",
                columns: new[] { "Id", "AccountNumber", "IBAN", "IsActive" },
                values: new object[] { 4, "1011562413", "NL22RABO1011562413", false });

            migrationBuilder.InsertData(
                table: "IbanStores",
                columns: new[] { "Id", "AccountNumber", "IBAN", "IsActive" },
                values: new object[] { 5, "5251802137", "NL60INGB5251802137", false });

            migrationBuilder.InsertData(
                table: "IbanStores",
                columns: new[] { "Id", "AccountNumber", "IBAN", "IsActive" },
                values: new object[] { 6, "4520711568", "NL34INGB4520711568", false });

            migrationBuilder.InsertData(
                table: "IbanStores",
                columns: new[] { "Id", "AccountNumber", "IBAN", "IsActive" },
                values: new object[] { 7, "1008270121", "NL64INGB1008270121", false });

            migrationBuilder.InsertData(
                table: "IbanStores",
                columns: new[] { "Id", "AccountNumber", "IBAN", "IsActive" },
                values: new object[] { 8, "3403751775", "NL77ABNA3403751775", false });

            migrationBuilder.InsertData(
                table: "IbanStores",
                columns: new[] { "Id", "AccountNumber", "IBAN", "IsActive" },
                values: new object[] { 9, "1011562413", "NL22RABO1011562413", false });

            migrationBuilder.InsertData(
                table: "IbanStores",
                columns: new[] { "Id", "AccountNumber", "IBAN", "IsActive" },
                values: new object[] { 10, "4293946624", "NL69ABNA4293946624", false });

            migrationBuilder.InsertData(
                table: "IbanStores",
                columns: new[] { "Id", "AccountNumber", "IBAN", "IsActive" },
                values: new object[] { 11, "2682297498", "NL11RABO2682297498", false });

            migrationBuilder.InsertData(
                table: "IbanStores",
                columns: new[] { "Id", "AccountNumber", "IBAN", "IsActive" },
                values: new object[] { 12, "2067756052", "NL62INGB2067756052", false });

            migrationBuilder.InsertData(
                table: "IbanStores",
                columns: new[] { "Id", "AccountNumber", "IBAN", "IsActive" },
                values: new object[] { 13, "2859779760", "NL55ABNA2859779760", false });

            migrationBuilder.InsertData(
                table: "IbanStores",
                columns: new[] { "Id", "AccountNumber", "IBAN", "IsActive" },
                values: new object[] { 14, "5055036109", "NL91INGB5055036109", false });

            migrationBuilder.InsertData(
                table: "IbanStores",
                columns: new[] { "Id", "AccountNumber", "IBAN", "IsActive" },
                values: new object[] { 15, "9589603858", "NL33RABO9589603858", false });

            migrationBuilder.InsertData(
                table: "IbanStores",
                columns: new[] { "Id", "AccountNumber", "IBAN", "IsActive" },
                values: new object[] { 16, "8902022560", "NL61ABNA8902022560", false });

            migrationBuilder.InsertData(
                table: "TransactionFees",
                columns: new[] { "Id", "CreateDate", "ModifyDate", "Percentage", "Type" },
                values: new object[] { "902d0151-483d-4ada-8320-5346afff69c1", new DateTime(2022, 9, 19, 2, 36, 2, 488, DateTimeKind.Local).AddTicks(5900), new DateTime(2022, 9, 19, 2, 36, 2, 488, DateTimeKind.Local).AddTicks(5950), 1m, 0 });

            migrationBuilder.InsertData(
                table: "TransactionLimits",
                columns: new[] { "Id", "CreateDate", "MaxAmount", "MinAmount", "ModifyDate", "Type" },
                values: new object[] { "809b3b27-fd3b-4f5c-a2e4-5ab989744afb", new DateTime(2022, 9, 19, 2, 36, 2, 488, DateTimeKind.Local).AddTicks(7090), 100m, 1m, new DateTime(2022, 9, 19, 2, 36, 2, 488, DateTimeKind.Local).AddTicks(7080), 0 });

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_HolderId",
                table: "Accounts",
                column: "HolderId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Accounts");

            migrationBuilder.DropTable(
                name: "IbanStores");

            migrationBuilder.DropTable(
                name: "TransactionFees");

            migrationBuilder.DropTable(
                name: "TransactionHistories");

            migrationBuilder.DropTable(
                name: "TransactionLimits");

            migrationBuilder.DropTable(
                name: "AccountHolders");
        }
    }
}
