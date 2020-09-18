using Microsoft.EntityFrameworkCore.Migrations;

namespace _4_Data.Migrations
{
    public partial class ChangedProjectModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AddColumn<int>(
                name: "ProjectIconId",
                table: "Project",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "UploaderId",
                table: "File",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_Project_ProjectIconId",
                table: "Project",
                column: "ProjectIconId");

            migrationBuilder.CreateIndex(
                name: "IX_File_UploaderId",
                table: "File",
                column: "UploaderId");

            migrationBuilder.AddForeignKey(
                name: "FK_File_User_UploaderId",
                table: "File",
                column: "UploaderId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Project_File_ProjectIconId",
                table: "Project",
                column: "ProjectIconId",
                principalTable: "File",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_File_User_UploaderId",
                table: "File");

            migrationBuilder.DropForeignKey(
                name: "FK_Project_File_ProjectIconId",
                table: "Project");

            migrationBuilder.DropIndex(
                name: "IX_Project_ProjectIconId",
                table: "Project");

            migrationBuilder.DropIndex(
                name: "IX_File_UploaderId",
                table: "File");

            migrationBuilder.DropColumn(
                name: "ProjectIconId",
                table: "Project");

            migrationBuilder.AddColumn<int>(
                name: "ProjectIconFileIdId",
                table: "Project",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "UploaderId",
                table: "File",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

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
    }
}
