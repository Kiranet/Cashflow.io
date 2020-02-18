using Microsoft.EntityFrameworkCore.Migrations;

namespace Cashflowio.Infrastructure.Migrations
{
    public partial class QuitarCamposInnecesarios : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AmountConverted",
                table: "RawTransactions");

            migrationBuilder.DropColumn(
                name: "CurrencyOfConversion",
                table: "RawTransactions");

            migrationBuilder.DropColumn(
                name: "Recurrence",
                table: "RawTransactions");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "AmountConverted",
                table: "RawTransactions",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "CurrencyOfConversion",
                table: "RawTransactions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Recurrence",
                table: "RawTransactions",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
