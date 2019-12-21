using Microsoft.EntityFrameworkCore.Migrations;

namespace Cashflowio.Infrastructure.Migrations
{
    public partial class OptionalCreditChargeForPayment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CreditPayments_CreditCharges_CreditChargeId",
                table: "CreditPayments");

            migrationBuilder.AlterColumn<int>(
                name: "CreditChargeId",
                table: "CreditPayments",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_CreditPayments_CreditCharges_CreditChargeId",
                table: "CreditPayments",
                column: "CreditChargeId",
                principalTable: "CreditCharges",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CreditPayments_CreditCharges_CreditChargeId",
                table: "CreditPayments");

            migrationBuilder.AlterColumn<int>(
                name: "CreditChargeId",
                table: "CreditPayments",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_CreditPayments_CreditCharges_CreditChargeId",
                table: "CreditPayments",
                column: "CreditChargeId",
                principalTable: "CreditCharges",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
