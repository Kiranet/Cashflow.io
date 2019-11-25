using Microsoft.EntityFrameworkCore.Migrations;

namespace Cashflowio.Infrastructure.Migrations
{
    public partial class IncomeSource2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Income_IncomeSources_IncomeSourceId",
                table: "Income");

            migrationBuilder.DropIndex(
                name: "IX_Income_IncomeSourceId",
                table: "Income");

            migrationBuilder.DropColumn(
                name: "IncomeSourceId",
                table: "Income");

            migrationBuilder.CreateIndex(
                name: "IX_Income_SourceId",
                table: "Income",
                column: "SourceId");

            migrationBuilder.AddForeignKey(
                name: "FK_Income_IncomeSources_SourceId",
                table: "Income",
                column: "SourceId",
                principalTable: "IncomeSources",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Income_IncomeSources_SourceId",
                table: "Income");

            migrationBuilder.DropIndex(
                name: "IX_Income_SourceId",
                table: "Income");

            migrationBuilder.AddColumn<int>(
                name: "IncomeSourceId",
                table: "Income",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Income_IncomeSourceId",
                table: "Income",
                column: "IncomeSourceId");

            migrationBuilder.AddForeignKey(
                name: "FK_Income_IncomeSources_IncomeSourceId",
                table: "Income",
                column: "IncomeSourceId",
                principalTable: "IncomeSources",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
