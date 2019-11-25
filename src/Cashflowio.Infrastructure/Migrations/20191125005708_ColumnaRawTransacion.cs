using Microsoft.EntityFrameworkCore.Migrations;

namespace Cashflowio.Infrastructure.Migrations
{
    public partial class ColumnaRawTransacion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RawTransactionId",
                table: "Transfers",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RawTransactionId",
                table: "Income",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Transfers_RawTransactionId",
                table: "Transfers",
                column: "RawTransactionId");

            migrationBuilder.CreateIndex(
                name: "IX_Income_RawTransactionId",
                table: "Income",
                column: "RawTransactionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Income_RawTransactions_RawTransactionId",
                table: "Income",
                column: "RawTransactionId",
                principalTable: "RawTransactions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Transfers_RawTransactions_RawTransactionId",
                table: "Transfers",
                column: "RawTransactionId",
                principalTable: "RawTransactions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Income_RawTransactions_RawTransactionId",
                table: "Income");

            migrationBuilder.DropForeignKey(
                name: "FK_Transfers_RawTransactions_RawTransactionId",
                table: "Transfers");

            migrationBuilder.DropIndex(
                name: "IX_Transfers_RawTransactionId",
                table: "Transfers");

            migrationBuilder.DropIndex(
                name: "IX_Income_RawTransactionId",
                table: "Income");

            migrationBuilder.DropColumn(
                name: "RawTransactionId",
                table: "Transfers");

            migrationBuilder.DropColumn(
                name: "RawTransactionId",
                table: "Income");
        }
    }
}
