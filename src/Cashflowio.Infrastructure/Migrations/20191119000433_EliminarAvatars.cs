using Microsoft.EntityFrameworkCore.Migrations;

namespace Cashflowio.Infrastructure.Migrations
{
    public partial class EliminarAvatars : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_IncomeSources_Avatars_AvatarId",
                table: "IncomeSources");

            migrationBuilder.DropForeignKey(
                name: "FK_MoneyAccounts_Avatars_AvatarId",
                table: "MoneyAccounts");

            migrationBuilder.DropTable(
                name: "Avatars");

            migrationBuilder.DropIndex(
                name: "IX_MoneyAccounts_AvatarId",
                table: "MoneyAccounts");

            migrationBuilder.DropIndex(
                name: "IX_IncomeSources_AvatarId",
                table: "IncomeSources");

            migrationBuilder.DropColumn(
                name: "AvatarId",
                table: "MoneyAccounts");

            migrationBuilder.DropColumn(
                name: "AvatarId",
                table: "IncomeSources");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AvatarId",
                table: "MoneyAccounts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "AvatarId",
                table: "IncomeSources",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Avatars",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Avatars", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MoneyAccounts_AvatarId",
                table: "MoneyAccounts",
                column: "AvatarId");

            migrationBuilder.CreateIndex(
                name: "IX_IncomeSources_AvatarId",
                table: "IncomeSources",
                column: "AvatarId");

            migrationBuilder.AddForeignKey(
                name: "FK_IncomeSources_Avatars_AvatarId",
                table: "IncomeSources",
                column: "AvatarId",
                principalTable: "Avatars",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MoneyAccounts_Avatars_AvatarId",
                table: "MoneyAccounts",
                column: "AvatarId",
                principalTable: "Avatars",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
