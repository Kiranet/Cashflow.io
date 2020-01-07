using Microsoft.EntityFrameworkCore.Migrations;

namespace Cashflowio.Infrastructure.Migrations
{
    public partial class BudgetCategories : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BudgetCategoryId",
                table: "ExpenseCategories",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "BudgetCategories",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BudgetCategories", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ExpenseCategories_BudgetCategoryId",
                table: "ExpenseCategories",
                column: "BudgetCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_ExpenseCategories_BudgetCategories_BudgetCategoryId",
                table: "ExpenseCategories",
                column: "BudgetCategoryId",
                principalTable: "BudgetCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExpenseCategories_BudgetCategories_BudgetCategoryId",
                table: "ExpenseCategories");

            migrationBuilder.DropTable(
                name: "BudgetCategories");

            migrationBuilder.DropIndex(
                name: "IX_ExpenseCategories_BudgetCategoryId",
                table: "ExpenseCategories");

            migrationBuilder.DropColumn(
                name: "BudgetCategoryId",
                table: "ExpenseCategories");
        }
    }
}
