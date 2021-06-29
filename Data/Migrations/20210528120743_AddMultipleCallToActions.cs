using Microsoft.EntityFrameworkCore.Migrations;

namespace _4_Data.Migrations
{
    public partial class AddMultipleCallToActions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Project_CallToAction_CallToActionId",
                table: "Project");

            migrationBuilder.DropIndex(
                name: "IX_Project_CallToActionId",
                table: "Project");

            migrationBuilder.DropColumn(
                name: "CallToActionId",
                table: "Project");

            migrationBuilder.AddColumn<int>(
                name: "ProjectId",
                table: "CallToAction",
                nullable: false);

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

            migrationBuilder.AddColumn<int>(
                name: "CallToActionId",
                table: "Project",
                type: "int",
                nullable: false);

            migrationBuilder.CreateIndex(
                name: "IX_Project_CallToActionId",
                table: "Project",
                column: "CallToActionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Project_CallToAction_CallToActionId",
                table: "Project",
                column: "CallToActionId",
                principalTable: "CallToAction",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
