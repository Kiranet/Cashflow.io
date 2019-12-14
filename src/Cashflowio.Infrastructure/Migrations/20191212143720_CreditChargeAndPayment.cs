using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Cashflowio.Infrastructure.Migrations
{
    public partial class CreditChargeAndPayment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CreditCharges",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(nullable: false),
                    Amount = table.Column<double>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    ExchangeRateId = table.Column<int>(nullable: true),
                    RawTransactionId = table.Column<int>(nullable: false),
                    SourceId = table.Column<int>(nullable: false),
                    DestinationId = table.Column<int>(nullable: false),
                    ConceptId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CreditCharges", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CreditCharges_Concepts_ConceptId",
                        column: x => x.ConceptId,
                        principalTable: "Concepts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CreditCharges_ExpenseCategories_DestinationId",
                        column: x => x.DestinationId,
                        principalTable: "ExpenseCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CreditCharges_ExchangeRates_ExchangeRateId",
                        column: x => x.ExchangeRateId,
                        principalTable: "ExchangeRates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CreditCharges_RawTransactions_RawTransactionId",
                        column: x => x.RawTransactionId,
                        principalTable: "RawTransactions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CreditCharges_MoneyAccounts_SourceId",
                        column: x => x.SourceId,
                        principalTable: "MoneyAccounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CreditPayments",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(nullable: false),
                    Amount = table.Column<double>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    ExchangeRateId = table.Column<int>(nullable: true),
                    RawTransactionId = table.Column<int>(nullable: false),
                    SourceId = table.Column<int>(nullable: false),
                    DestinationId = table.Column<int>(nullable: false),
                    CreditChargeId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CreditPayments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CreditPayments_CreditCharges_CreditChargeId",
                        column: x => x.CreditChargeId,
                        principalTable: "CreditCharges",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CreditPayments_MoneyAccounts_DestinationId",
                        column: x => x.DestinationId,
                        principalTable: "MoneyAccounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_CreditPayments_ExchangeRates_ExchangeRateId",
                        column: x => x.ExchangeRateId,
                        principalTable: "ExchangeRates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CreditPayments_RawTransactions_RawTransactionId",
                        column: x => x.RawTransactionId,
                        principalTable: "RawTransactions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_CreditPayments_MoneyAccounts_SourceId",
                        column: x => x.SourceId,
                        principalTable: "MoneyAccounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CreditCharges_ConceptId",
                table: "CreditCharges",
                column: "ConceptId");

            migrationBuilder.CreateIndex(
                name: "IX_CreditCharges_DestinationId",
                table: "CreditCharges",
                column: "DestinationId");

            migrationBuilder.CreateIndex(
                name: "IX_CreditCharges_ExchangeRateId",
                table: "CreditCharges",
                column: "ExchangeRateId");

            migrationBuilder.CreateIndex(
                name: "IX_CreditCharges_RawTransactionId",
                table: "CreditCharges",
                column: "RawTransactionId");

            migrationBuilder.CreateIndex(
                name: "IX_CreditCharges_SourceId",
                table: "CreditCharges",
                column: "SourceId");

            migrationBuilder.CreateIndex(
                name: "IX_CreditPayments_CreditChargeId",
                table: "CreditPayments",
                column: "CreditChargeId");

            migrationBuilder.CreateIndex(
                name: "IX_CreditPayments_DestinationId",
                table: "CreditPayments",
                column: "DestinationId");

            migrationBuilder.CreateIndex(
                name: "IX_CreditPayments_ExchangeRateId",
                table: "CreditPayments",
                column: "ExchangeRateId");

            migrationBuilder.CreateIndex(
                name: "IX_CreditPayments_RawTransactionId",
                table: "CreditPayments",
                column: "RawTransactionId");

            migrationBuilder.CreateIndex(
                name: "IX_CreditPayments_SourceId",
                table: "CreditPayments",
                column: "SourceId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CreditPayments");

            migrationBuilder.DropTable(
                name: "CreditCharges");
        }
    }
}
