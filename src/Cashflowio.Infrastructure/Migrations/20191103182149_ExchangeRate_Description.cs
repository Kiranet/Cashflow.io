using Microsoft.EntityFrameworkCore.Migrations;

namespace Cashflowio.Infrastructure.Migrations
{
    public partial class ExchangeRate_Description : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "ExchangeRates",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "ExchangeRates");
        }
    }
}
