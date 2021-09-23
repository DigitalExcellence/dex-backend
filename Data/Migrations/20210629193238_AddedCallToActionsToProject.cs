using Microsoft.EntityFrameworkCore.Migrations;

namespace _4_Data.Migrations
{
    public partial class AddedCallToActionsToProject : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProjectId",
                table: "CallToAction",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CallToAction_ProjectId",
                table: "CallToAction",
                column: "ProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_CallToAction_Project_ProjectId",
                table: "CallToAction",
                column: "ProjectId",
                principalTable: "Project",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CallToAction_Project_ProjectId",
                table: "CallToAction");

            migrationBuilder.DropIndex(
                name: "IX_CallToAction_ProjectId",
                table: "CallToAction");

            migrationBuilder.DropColumn(
                name: "ProjectId",
                table: "CallToAction");
        }
    }
}
