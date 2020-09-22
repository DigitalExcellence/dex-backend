using Microsoft.EntityFrameworkCore.Migrations;

namespace _4_Data.Migrations
{
    public partial class AddedDBSetForUserFollowedProjects : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserFollowedProject_Project_ProjectId",
                table: "UserFollowedProject");

            migrationBuilder.DropForeignKey(
                name: "FK_UserFollowedProject_User_UserId",
                table: "UserFollowedProject");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserFollowedProject",
                table: "UserFollowedProject");

            migrationBuilder.RenameTable(
                name: "UserFollowedProject",
                newName: "UserFollowedProjects");

            migrationBuilder.RenameIndex(
                name: "IX_UserFollowedProject_UserId",
                table: "UserFollowedProjects",
                newName: "IX_UserFollowedProjects_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_UserFollowedProject_ProjectId",
                table: "UserFollowedProjects",
                newName: "IX_UserFollowedProjects_ProjectId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserFollowedProjects",
                table: "UserFollowedProjects",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserFollowedProjects_Project_ProjectId",
                table: "UserFollowedProjects",
                column: "ProjectId",
                principalTable: "Project",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserFollowedProjects_User_UserId",
                table: "UserFollowedProjects",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserFollowedProjects_Project_ProjectId",
                table: "UserFollowedProjects");

            migrationBuilder.DropForeignKey(
                name: "FK_UserFollowedProjects_User_UserId",
                table: "UserFollowedProjects");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserFollowedProjects",
                table: "UserFollowedProjects");

            migrationBuilder.RenameTable(
                name: "UserFollowedProjects",
                newName: "UserFollowedProject");

            migrationBuilder.RenameIndex(
                name: "IX_UserFollowedProjects_UserId",
                table: "UserFollowedProject",
                newName: "IX_UserFollowedProject_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_UserFollowedProjects_ProjectId",
                table: "UserFollowedProject",
                newName: "IX_UserFollowedProject_ProjectId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserFollowedProject",
                table: "UserFollowedProject",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserFollowedProject_Project_ProjectId",
                table: "UserFollowedProject",
                column: "ProjectId",
                principalTable: "Project",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserFollowedProject_User_UserId",
                table: "UserFollowedProject",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
