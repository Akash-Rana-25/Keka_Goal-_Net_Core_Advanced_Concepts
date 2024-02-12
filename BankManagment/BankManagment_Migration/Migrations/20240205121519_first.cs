using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BankManagment_Migration.Migrations
{
    public partial class first : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AccountTypes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PaymentMethods",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentMethods", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BankAccounts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MiddleName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AccountNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OpeningDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ClosingDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AccountTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BankAccounts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BankAccounts_AccountTypes_AccountTypeId",
                        column: x => x.AccountTypeId,
                        principalTable: "AccountTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BankTransactions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TransactionPersonFirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TransactionPersonMiddleName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TransactionPersonLastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TransactionType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Category = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TransactionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PaymentMethodID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BankAccountID = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BankTransactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BankTransactions_BankAccounts_BankAccountID",
                        column: x => x.BankAccountID,
                        principalTable: "BankAccounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BankTransactions_PaymentMethods_PaymentMethodID",
                        column: x => x.PaymentMethodID,
                        principalTable: "PaymentMethods",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AccountTypes",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("5ff8edd6-cf7b-457d-b8ed-afce059b55c7"), "Asset" },
                    { new Guid("81d80448-889d-4091-926a-bf75f40de06a"), "Liability" }
                });

            migrationBuilder.InsertData(
                table: "PaymentMethods",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("557554ed-c3d5-4788-82c8-13c1763733b7"), "NEFT" },
                    { new Guid("5985ddc8-4579-40f2-945b-6c1fd9aa486e"), "Cheque" },
                    { new Guid("8a51ce61-3e24-4fdc-9723-f880618bb1b1"), "Cash" },
                    { new Guid("c52b16e2-748c-4058-80b7-439744846f4b"), "Other" },
                    { new Guid("d1eac9cd-00f3-4859-8e62-6563573fb473"), "RTGS" }
                });

            migrationBuilder.InsertData(
                table: "BankAccounts",
                columns: new[] { "Id", "AccountNumber", "AccountTypeId", "ClosingDate", "FirstName", "LastName", "MiddleName", "OpeningDate" },
                values: new object[,]
                {
                    { new Guid("20d3d291-4510-4c14-826c-76161b68658f"), "62811879", new Guid("81d80448-889d-4091-926a-bf75f40de06a"), null, "Akash", "Rana", null, new DateTime(2024, 2, 5, 17, 45, 19, 777, DateTimeKind.Local).AddTicks(5494) },
                    { new Guid("33a8604c-b943-47f2-bb3f-571ebc217243"), "92412811", new Guid("81d80448-889d-4091-926a-bf75f40de06a"), null, "Akash", "Rana", null, new DateTime(2024, 2, 3, 17, 45, 19, 777, DateTimeKind.Local).AddTicks(5517) },
                    { new Guid("57dba9c8-c7ed-4d23-b40f-3d6ffef4c718"), "93068980", new Guid("81d80448-889d-4091-926a-bf75f40de06a"), null, "Akash", "Rana", null, new DateTime(2024, 2, 1, 17, 45, 19, 777, DateTimeKind.Local).AddTicks(5525) },
                    { new Guid("70e6fb04-786f-423e-887d-4cb034d94a4e"), "62762801", new Guid("5ff8edd6-cf7b-457d-b8ed-afce059b55c7"), null, "Akash", "Rana", null, new DateTime(2024, 2, 4, 17, 45, 19, 777, DateTimeKind.Local).AddTicks(5514) },
                    { new Guid("b88d6232-856b-4fe8-9943-2e93227d2903"), "72944215", new Guid("81d80448-889d-4091-926a-bf75f40de06a"), null, "Akash", "Rana", null, new DateTime(2024, 2, 2, 17, 45, 19, 777, DateTimeKind.Local).AddTicks(5522) }
                });

            migrationBuilder.InsertData(
                table: "BankTransactions",
                columns: new[] { "Id", "Amount", "BankAccountID", "Category", "PaymentMethodID", "TransactionDate", "TransactionPersonFirstName", "TransactionPersonLastName", "TransactionPersonMiddleName", "TransactionType" },
                values: new object[,]
                {
                    { new Guid("0ce2f033-0687-4e74-9da9-8ff8adefe545"), 440.55853118899000m, new Guid("b88d6232-856b-4fe8-9943-2e93227d2903"), "Bank Interest", new Guid("557554ed-c3d5-4788-82c8-13c1763733b7"), new DateTime(2024, 2, 1, 17, 45, 19, 777, DateTimeKind.Local).AddTicks(5682), "Akash", "Rana", null, "Credit" },
                    { new Guid("560847ff-6f5b-4209-a7a8-c1b0c3ce6922"), 887.781450078504000m, new Guid("b88d6232-856b-4fe8-9943-2e93227d2903"), "Bank Interest", new Guid("8a51ce61-3e24-4fdc-9723-f880618bb1b1"), new DateTime(2024, 2, 2, 17, 45, 19, 777, DateTimeKind.Local).AddTicks(5679), "Akash", "Rana", null, "Debit" },
                    { new Guid("60007847-0969-4a0c-a5e2-f759d523eb0c"), 10.4129740680303000m, new Guid("57dba9c8-c7ed-4d23-b40f-3d6ffef4c718"), "Bank Interest", new Guid("557554ed-c3d5-4788-82c8-13c1763733b7"), new DateTime(2024, 2, 3, 17, 45, 19, 777, DateTimeKind.Local).AddTicks(5676), "Akash", "Rana", null, "Credit" },
                    { new Guid("7220200c-f8b5-406b-af0c-57759b1d2c27"), 651.025806069377000m, new Guid("57dba9c8-c7ed-4d23-b40f-3d6ffef4c718"), "Bank Interest", new Guid("c52b16e2-748c-4058-80b7-439744846f4b"), new DateTime(2024, 2, 4, 17, 45, 19, 777, DateTimeKind.Local).AddTicks(5670), "Akash", "Rana", null, "Debit" },
                    { new Guid("8706e253-6fb9-4aaf-9a8c-06230b025a76"), 180.135361754793000m, new Guid("33a8604c-b943-47f2-bb3f-571ebc217243"), "Bank Charges", new Guid("c52b16e2-748c-4058-80b7-439744846f4b"), new DateTime(2024, 2, 5, 17, 45, 19, 777, DateTimeKind.Local).AddTicks(5665), "Akash", "Rana", null, "Credit" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_BankAccounts_AccountTypeId",
                table: "BankAccounts",
                column: "AccountTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_BankTransactions_BankAccountID",
                table: "BankTransactions",
                column: "BankAccountID");

            migrationBuilder.CreateIndex(
                name: "IX_BankTransactions_PaymentMethodID",
                table: "BankTransactions",
                column: "PaymentMethodID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BankTransactions");

            migrationBuilder.DropTable(
                name: "BankAccounts");

            migrationBuilder.DropTable(
                name: "PaymentMethods");

            migrationBuilder.DropTable(
                name: "AccountTypes");
        }
    }
}
