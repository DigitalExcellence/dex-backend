using Microsoft.EntityFrameworkCore.Migrations;

namespace _4_Data.Migrations
{
    public partial class UpdatedUserModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserFollowedProjects_User_UserId",
                table: "UserFollowedProjects");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "UserFollowedProjects",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_UserFollowedProjects_User_UserId",
                table: "UserFollowedProjects",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserFollowedProjects_User_UserId",
                table: "UserFollowedProjects");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "UserFollowedProjects",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_UserFollowedProjects_User_UserId",
                table: "UserFollowedProjects",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
