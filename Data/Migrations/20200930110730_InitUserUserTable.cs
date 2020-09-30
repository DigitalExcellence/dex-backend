using Microsoft.EntityFrameworkCore.Migrations;

namespace _4_Data.Migrations
{
    public partial class InitUserUserTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserUser",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(nullable: true),
                    FollowedUserId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserUser", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserUser_User_FollowedUserId",
                        column: x => x.FollowedUserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserUser_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserUser_FollowedUserId",
                table: "UserUser",
                column: "FollowedUserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserUser_UserId",
                table: "UserUser",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserUser");
        }
    }
}
