using Microsoft.EntityFrameworkCore.Migrations;

namespace _4_Data.Migrations
{
    public partial class portfolioItemUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PortfolioItem_Project_ProjectId",
                table: "PortfolioItem");

            migrationBuilder.DropIndex(
                name: "IX_PortfolioItem_ProjectId",
                table: "PortfolioItem");

            migrationBuilder.AlterColumn<int>(
                name: "ProjectId",
                table: "PortfolioItem",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "ProjectId",
                table: "PortfolioItem",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.CreateIndex(
                name: "IX_PortfolioItem_ProjectId",
                table: "PortfolioItem",
                column: "ProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_PortfolioItem_Project_ProjectId",
                table: "PortfolioItem",
                column: "ProjectId",
                principalTable: "Project",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
