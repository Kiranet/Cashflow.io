using Microsoft.EntityFrameworkCore.Migrations;

namespace Cashflowio.Infrastructure.Migrations
{
    public partial class IncomeSource : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IncomeSourceId",
                table: "Income",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SourceId",
                table: "Income",
                nullable: false,
                defaultValue: 0);

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

        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropColumn(
                name: "SourceId",
                table: "Income");
        }
    }
}
