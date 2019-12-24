using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Cashflowio.Infrastructure.Migrations
{
    public partial class Transactions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                "Concepts",
                table => new
                {
                    Id = table.Column<int>()
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table => { table.PrimaryKey("PK_Concepts", x => x.Id); });

            migrationBuilder.CreateTable(
                "ExchangeRates",
                table => new
                {
                    Id = table.Column<int>()
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(),
                    Value = table.Column<double>(),
                    Currency = table.Column<int>(),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table => { table.PrimaryKey("PK_ExchangeRates", x => x.Id); });

            migrationBuilder.CreateTable(
                "ExpenseCategories",
                table => new
                {
                    Id = table.Column<int>()
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table => { table.PrimaryKey("PK_ExpenseCategories", x => x.Id); });

            migrationBuilder.CreateTable(
                "IncomeSources",
                table => new
                {
                    Id = table.Column<int>()
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    Currency = table.Column<string>(nullable: true),
                    Concepts = table.Column<string>(nullable: true)
                },
                constraints: table => { table.PrimaryKey("PK_IncomeSources", x => x.Id); });

            migrationBuilder.CreateTable(
                "MoneyAccounts",
                table => new
                {
                    Id = table.Column<int>()
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    Currency = table.Column<string>(nullable: true),
                    Type = table.Column<string>(nullable: true)
                },
                constraints: table => { table.PrimaryKey("PK_MoneyAccounts", x => x.Id); });

            migrationBuilder.CreateTable(
                "RawTransactions",
                table => new
                {
                    Id = table.Column<int>()
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(),
                    Type = table.Column<string>(nullable: true),
                    Amount = table.Column<double>(),
                    Source = table.Column<string>(nullable: true),
                    Destination = table.Column<string>(nullable: true),
                    Tag = table.Column<string>(nullable: true),
                    Note = table.Column<string>(nullable: true),
                    Recurrence = table.Column<string>(nullable: true),
                    CurrencyOfConversion = table.Column<string>(nullable: true),
                    AmountConverted = table.Column<double>(),
                    Currency = table.Column<string>(nullable: true),
                    IsProcessed = table.Column<bool>()
                },
                constraints: table => { table.PrimaryKey("PK_RawTransactions", x => x.Id); });

            migrationBuilder.CreateTable(
                "CreditCharges",
                table => new
                {
                    Id = table.Column<int>()
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(),
                    Amount = table.Column<double>(),
                    Description = table.Column<string>(nullable: true),
                    ExchangeRateId = table.Column<int>(nullable: true),
                    RawTransactionId = table.Column<int>(),
                    SourceId = table.Column<int>(),
                    DestinationId = table.Column<int>(),
                    ConceptId = table.Column<int>()
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CreditCharges", x => x.Id);
                    table.ForeignKey(
                        "FK_CreditCharges_Concepts_ConceptId",
                        x => x.ConceptId,
                        "Concepts",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        "FK_CreditCharges_ExpenseCategories_DestinationId",
                        x => x.DestinationId,
                        "ExpenseCategories",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        "FK_CreditCharges_ExchangeRates_ExchangeRateId",
                        x => x.ExchangeRateId,
                        "ExchangeRates",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        "FK_CreditCharges_RawTransactions_RawTransactionId",
                        x => x.RawTransactionId,
                        "RawTransactions",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        "FK_CreditCharges_MoneyAccounts_SourceId",
                        x => x.SourceId,
                        "MoneyAccounts",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                "Expenses",
                table => new
                {
                    Id = table.Column<int>()
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(),
                    Amount = table.Column<double>(),
                    Description = table.Column<string>(nullable: true),
                    ExchangeRateId = table.Column<int>(nullable: true),
                    RawTransactionId = table.Column<int>(),
                    SourceId = table.Column<int>(),
                    DestinationId = table.Column<int>(),
                    ConceptId = table.Column<int>(),
                    Currency = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Expenses", x => x.Id);
                    table.ForeignKey(
                        "FK_Expenses_Concepts_ConceptId",
                        x => x.ConceptId,
                        "Concepts",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        "FK_Expenses_ExpenseCategories_DestinationId",
                        x => x.DestinationId,
                        "ExpenseCategories",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        "FK_Expenses_ExchangeRates_ExchangeRateId",
                        x => x.ExchangeRateId,
                        "ExchangeRates",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        "FK_Expenses_RawTransactions_RawTransactionId",
                        x => x.RawTransactionId,
                        "RawTransactions",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        "FK_Expenses_MoneyAccounts_SourceId",
                        x => x.SourceId,
                        "MoneyAccounts",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                "Income",
                table => new
                {
                    Id = table.Column<int>()
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(),
                    Amount = table.Column<double>(),
                    Description = table.Column<string>(nullable: true),
                    ExchangeRateId = table.Column<int>(nullable: true),
                    RawTransactionId = table.Column<int>(),
                    SourceId = table.Column<int>(),
                    DestinationId = table.Column<int>()
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Income", x => x.Id);
                    table.ForeignKey(
                        "FK_Income_MoneyAccounts_DestinationId",
                        x => x.DestinationId,
                        "MoneyAccounts",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        "FK_Income_ExchangeRates_ExchangeRateId",
                        x => x.ExchangeRateId,
                        "ExchangeRates",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        "FK_Income_RawTransactions_RawTransactionId",
                        x => x.RawTransactionId,
                        "RawTransactions",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        "FK_Income_IncomeSources_SourceId",
                        x => x.SourceId,
                        "IncomeSources",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                "Transfers",
                table => new
                {
                    Id = table.Column<int>()
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(),
                    Amount = table.Column<double>(),
                    Description = table.Column<string>(nullable: true),
                    ExchangeRateId = table.Column<int>(nullable: true),
                    RawTransactionId = table.Column<int>(),
                    SourceId = table.Column<int>(),
                    DestinationId = table.Column<int>(),
                    Type = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transfers", x => x.Id);
                    table.ForeignKey(
                        "FK_Transfers_MoneyAccounts_DestinationId",
                        x => x.DestinationId,
                        "MoneyAccounts",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        "FK_Transfers_ExchangeRates_ExchangeRateId",
                        x => x.ExchangeRateId,
                        "ExchangeRates",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        "FK_Transfers_RawTransactions_RawTransactionId",
                        x => x.RawTransactionId,
                        "RawTransactions",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        "FK_Transfers_MoneyAccounts_SourceId",
                        x => x.SourceId,
                        "MoneyAccounts",
                        "Id");
                });

            migrationBuilder.CreateTable(
                "CreditPayments",
                table => new
                {
                    Id = table.Column<int>()
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(),
                    Amount = table.Column<double>(),
                    Description = table.Column<string>(nullable: true),
                    ExchangeRateId = table.Column<int>(nullable: true),
                    RawTransactionId = table.Column<int>(),
                    SourceId = table.Column<int>(),
                    DestinationId = table.Column<int>(),
                    CreditChargeId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CreditPayments", x => x.Id);
                    table.ForeignKey(
                        "FK_CreditPayments_CreditCharges_CreditChargeId",
                        x => x.CreditChargeId,
                        "CreditCharges",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        "FK_CreditPayments_MoneyAccounts_DestinationId",
                        x => x.DestinationId,
                        "MoneyAccounts",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        "FK_CreditPayments_ExchangeRates_ExchangeRateId",
                        x => x.ExchangeRateId,
                        "ExchangeRates",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        "FK_CreditPayments_RawTransactions_RawTransactionId",
                        x => x.RawTransactionId,
                        "RawTransactions",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        "FK_CreditPayments_MoneyAccounts_SourceId",
                        x => x.SourceId,
                        "MoneyAccounts",
                        "Id");
                });

            migrationBuilder.CreateIndex(
                "IX_CreditCharges_ConceptId",
                "CreditCharges",
                "ConceptId");

            migrationBuilder.CreateIndex(
                "IX_CreditCharges_DestinationId",
                "CreditCharges",
                "DestinationId");

            migrationBuilder.CreateIndex(
                "IX_CreditCharges_ExchangeRateId",
                "CreditCharges",
                "ExchangeRateId");

            migrationBuilder.CreateIndex(
                "IX_CreditCharges_RawTransactionId",
                "CreditCharges",
                "RawTransactionId");

            migrationBuilder.CreateIndex(
                "IX_CreditCharges_SourceId",
                "CreditCharges",
                "SourceId");

            migrationBuilder.CreateIndex(
                "IX_CreditPayments_CreditChargeId",
                "CreditPayments",
                "CreditChargeId");

            migrationBuilder.CreateIndex(
                "IX_CreditPayments_DestinationId",
                "CreditPayments",
                "DestinationId");

            migrationBuilder.CreateIndex(
                "IX_CreditPayments_ExchangeRateId",
                "CreditPayments",
                "ExchangeRateId");

            migrationBuilder.CreateIndex(
                "IX_CreditPayments_RawTransactionId",
                "CreditPayments",
                "RawTransactionId");

            migrationBuilder.CreateIndex(
                "IX_CreditPayments_SourceId",
                "CreditPayments",
                "SourceId");

            migrationBuilder.CreateIndex(
                "IX_Expenses_ConceptId",
                "Expenses",
                "ConceptId");

            migrationBuilder.CreateIndex(
                "IX_Expenses_DestinationId",
                "Expenses",
                "DestinationId");

            migrationBuilder.CreateIndex(
                "IX_Expenses_ExchangeRateId",
                "Expenses",
                "ExchangeRateId");

            migrationBuilder.CreateIndex(
                "IX_Expenses_RawTransactionId",
                "Expenses",
                "RawTransactionId");

            migrationBuilder.CreateIndex(
                "IX_Expenses_SourceId",
                "Expenses",
                "SourceId");

            migrationBuilder.CreateIndex(
                "IX_Income_DestinationId",
                "Income",
                "DestinationId");

            migrationBuilder.CreateIndex(
                "IX_Income_ExchangeRateId",
                "Income",
                "ExchangeRateId");

            migrationBuilder.CreateIndex(
                "IX_Income_RawTransactionId",
                "Income",
                "RawTransactionId");

            migrationBuilder.CreateIndex(
                "IX_Income_SourceId",
                "Income",
                "SourceId");

            migrationBuilder.CreateIndex(
                "IX_Transfers_DestinationId",
                "Transfers",
                "DestinationId");

            migrationBuilder.CreateIndex(
                "IX_Transfers_ExchangeRateId",
                "Transfers",
                "ExchangeRateId");

            migrationBuilder.CreateIndex(
                "IX_Transfers_RawTransactionId",
                "Transfers",
                "RawTransactionId");

            migrationBuilder.CreateIndex(
                "IX_Transfers_SourceId",
                "Transfers",
                "SourceId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                "CreditPayments");

            migrationBuilder.DropTable(
                "Expenses");

            migrationBuilder.DropTable(
                "Income");

            migrationBuilder.DropTable(
                "Transfers");

            migrationBuilder.DropTable(
                "CreditCharges");

            migrationBuilder.DropTable(
                "IncomeSources");

            migrationBuilder.DropTable(
                "Concepts");

            migrationBuilder.DropTable(
                "ExpenseCategories");

            migrationBuilder.DropTable(
                "ExchangeRates");

            migrationBuilder.DropTable(
                "RawTransactions");

            migrationBuilder.DropTable(
                "MoneyAccounts");
        }
    }
}