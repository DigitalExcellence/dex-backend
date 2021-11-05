using Microsoft.EntityFrameworkCore.Migrations;

namespace _4_Data.Migrations
{

    public partial class AddFollowUsersAndProjects : Migration
    {

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable("UserProject",
                                         table => new
                                                  {
                                                      Id = table.Column<int>(nullable: false)
                                                                .Annotation("SqlServer:Identity", "1, 1"),
                                                      ProjectId = table.Column<int>(nullable: true),
                                                      UserId = table.Column<int>(nullable: false)
                                                  },
                                         constraints: table =>
                                         {
                                             table.PrimaryKey("PK_UserProject", x => x.Id);
                                             table.ForeignKey("FK_UserProject_Project_ProjectId",
                                                              x => x.ProjectId,
                                                              "Project",
                                                              "Id",
                                                              onDelete: ReferentialAction.Restrict);
                                             table.ForeignKey("FK_UserProject_User_UserId",
                                                              x => x.UserId,
                                                              "User",
                                                              "Id",
                                                              onDelete: ReferentialAction.Cascade);
                                         });

            migrationBuilder.CreateTable("UserUser",
                                         table => new
                                                  {
                                                      Id = table.Column<int>(nullable: false)
                                                                .Annotation("SqlServer:Identity", "1, 1"),
                                                      UserId = table.Column<int>(nullable: true),
                                                      FollowedUserId = table.Column<int>(nullable: false)
                                                  },
                                         constraints: table =>
                                         {
                                             table.PrimaryKey("PK_UserUser", x => x.Id);
                                             table.ForeignKey("FK_UserUser_User_FollowedUserId",
                                                              x => x.FollowedUserId,
                                                              "User",
                                                              "Id",
                                                              onDelete: ReferentialAction.Cascade);
                                             table.ForeignKey("FK_UserUser_User_UserId",
                                                              x => x.UserId,
                                                              "User",
                                                              "Id",
                                                              onDelete: ReferentialAction.Restrict);
                                         });

            migrationBuilder.CreateIndex("IX_UserProject_ProjectId",
                                         "UserProject",
                                         "ProjectId");

            migrationBuilder.CreateIndex("IX_UserProject_UserId",
                                         "UserProject",
                                         "UserId");

            migrationBuilder.CreateIndex("IX_UserUser_FollowedUserId",
                                         "UserUser",
                                         "FollowedUserId");

            migrationBuilder.CreateIndex("IX_UserUser_UserId",
                                         "UserUser",
                                         "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable("UserProject");

            migrationBuilder.DropTable("UserUser");
        }

    }

}
