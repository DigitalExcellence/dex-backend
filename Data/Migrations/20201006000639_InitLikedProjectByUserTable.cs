using Microsoft.EntityFrameworkCore.Migrations;

namespace _4_Data.Migrations
{

    public partial class InitLikedProjectByUserTable : Migration
    {

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable("ProjectLike",
                                         table => new
                                                  {
                                                      Id = table.Column<int>(nullable: false)
                                                                .Annotation("SqlServer:Identity", "1, 1"),
                                                      LikedProjectId = table.Column<int>(nullable: true),
                                                      UserId = table.Column<int>(nullable: false)
                                                  },
                                         constraints: table =>
                                         {
                                             table.PrimaryKey("PK_LikedProjectByUser", x => x.Id);
                                             table.ForeignKey("FK_LikedProjectByUser_Project_LikedProjectId",
                                                              x => x.LikedProjectId,
                                                              "Project",
                                                              "Id",
                                                              onDelete: ReferentialAction.Restrict);
                                             table.ForeignKey("FK_LikedProjectByUser_User_UserId",
                                                              x => x.UserId,
                                                              "User",
                                                              "Id",
                                                              onDelete: ReferentialAction.Cascade);
                                         });

            migrationBuilder.CreateIndex("IX_LikedProjectByUser_LikedProjectId",
                                         "ProjectLike",
                                         "LikedProjectId");

            migrationBuilder.CreateIndex("IX_LikedProjectByUser_UserId",
                                         "ProjectLike",
                                         "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable("ProjectLike");
        }

    }

}
