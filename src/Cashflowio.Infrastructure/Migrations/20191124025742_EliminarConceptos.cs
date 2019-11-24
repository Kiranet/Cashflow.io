using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Cashflowio.Infrastructure.Migrations
{
    public partial class EliminarConceptos : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Concepts");

            migrationBuilder.DropColumn(
                name: "EndDate",
                table: "IncomeSources");

            migrationBuilder.DropColumn(
                name: "IsFixed",
                table: "IncomeSources");

            migrationBuilder.DropColumn(
                name: "Recurrence",
                table: "IncomeSources");

            migrationBuilder.DropColumn(
                name: "StartDate",
                table: "IncomeSources");

            migrationBuilder.AddColumn<string>(
                name: "Concepts",
                table: "IncomeSources",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Concepts",
                table: "IncomeSources");

            migrationBuilder.AddColumn<DateTime>(
                name: "EndDate",
                table: "IncomeSources",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsFixed",
                table: "IncomeSources",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "Recurrence",
                table: "IncomeSources",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "StartDate",
                table: "IncomeSources",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateTable(
                name: "Concepts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Amount = table.Column<double>(type: "float", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DestinationId = table.Column<int>(type: "int", nullable: false),
                    ExchangeRateId = table.Column<int>(type: "int", nullable: true),
                    IncomeSourceId = table.Column<int>(type: "int", nullable: true),
                    PayDay = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Concepts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Concepts_MoneyAccounts_DestinationId",
                        column: x => x.DestinationId,
                        principalTable: "MoneyAccounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Concepts_ExchangeRates_ExchangeRateId",
                        column: x => x.ExchangeRateId,
                        principalTable: "ExchangeRates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Concepts_IncomeSources_IncomeSourceId",
                        column: x => x.IncomeSourceId,
                        principalTable: "IncomeSources",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Concepts_DestinationId",
                table: "Concepts",
                column: "DestinationId");

            migrationBuilder.CreateIndex(
                name: "IX_Concepts_ExchangeRateId",
                table: "Concepts",
                column: "ExchangeRateId");

            migrationBuilder.CreateIndex(
                name: "IX_Concepts_IncomeSourceId",
                table: "Concepts",
                column: "IncomeSourceId");
        }
    }
}
