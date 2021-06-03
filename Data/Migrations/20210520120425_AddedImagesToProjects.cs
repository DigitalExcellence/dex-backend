using Microsoft.EntityFrameworkCore.Migrations;

namespace _4_Data.Migrations
{
    public partial class AddedImagesToProjects : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProjectId",
                table: "File",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_File_ProjectId",
                table: "File",
                column: "ProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_File_Project_ProjectId",
                table: "File",
                column: "ProjectId",
                principalTable: "Project",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_File_Project_ProjectId",
                table: "File");

            migrationBuilder.DropIndex(
                name: "IX_File_ProjectId",
                table: "File");

            migrationBuilder.DropColumn(
                name: "ProjectId",
                table: "File");
        }
    }
}
