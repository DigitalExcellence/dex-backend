using Microsoft.EntityFrameworkCore.Migrations;

namespace _4_Data.Migrations
{
    public partial class ChangeUserProjectsTableToUserProject : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserProjects_Project_ProjectId",
                table: "UserProjects");

            migrationBuilder.DropForeignKey(
                name: "FK_UserProjects_User_UserId",
                table: "UserProjects");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserProjects",
                table: "UserProjects");

            migrationBuilder.RenameTable(
                name: "UserProjects",
                newName: "UserProject");

            migrationBuilder.RenameIndex(
                name: "IX_UserProjects_UserId",
                table: "UserProject",
                newName: "IX_UserProject_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_UserProjects_ProjectId",
                table: "UserProject",
                newName: "IX_UserProject_ProjectId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserProject",
                table: "UserProject",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserProject_Project_ProjectId",
                table: "UserProject",
                column: "ProjectId",
                principalTable: "Project",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserProject_User_UserId",
                table: "UserProject",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserProject_Project_ProjectId",
                table: "UserProject");

            migrationBuilder.DropForeignKey(
                name: "FK_UserProject_User_UserId",
                table: "UserProject");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserProject",
                table: "UserProject");

            migrationBuilder.RenameTable(
                name: "UserProject",
                newName: "UserProjects");

            migrationBuilder.RenameIndex(
                name: "IX_UserProject_UserId",
                table: "UserProjects",
                newName: "IX_UserProjects_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_UserProject_ProjectId",
                table: "UserProjects",
                newName: "IX_UserProjects_ProjectId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserProjects",
                table: "UserProjects",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserProjects_Project_ProjectId",
                table: "UserProjects",
                column: "ProjectId",
                principalTable: "Project",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserProjects_User_UserId",
                table: "UserProjects",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
