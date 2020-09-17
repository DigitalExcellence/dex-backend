using Microsoft.EntityFrameworkCore.Migrations;

namespace _4_Data.Migrations
{
    public partial class AddFileReferenceToProjectTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProjectIconFileIdId",
                table: "Project",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Project_ProjectIconFileIdId",
                table: "Project",
                column: "ProjectIconFileIdId");

            migrationBuilder.AddForeignKey(
                name: "FK_Project_File_ProjectIconFileIdId",
                table: "Project",
                column: "ProjectIconFileIdId",
                principalTable: "File",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Project_File_ProjectIconFileIdId",
                table: "Project");

            migrationBuilder.DropIndex(
                name: "IX_Project_ProjectIconFileIdId",
                table: "Project");

            migrationBuilder.DropColumn(
                name: "ProjectIconFileIdId",
                table: "Project");
        }
    }
}
