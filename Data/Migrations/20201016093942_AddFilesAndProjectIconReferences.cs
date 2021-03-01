using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace _4_Data.Migrations
{

    public partial class AddFilesAndProjectIconReferences : Migration
    {

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>("ProjectIconId",
                                            "Project",
                                            nullable: true);

            migrationBuilder.CreateTable("File",
                                         table => new
                                                  {
                                                      Id = table.Column<int>(nullable: false)
                                                                .Annotation("SqlServer:Identity", "1, 1"),
                                                      Path = table.Column<string>(nullable: false),
                                                      UploadDateTime = table.Column<DateTime>(nullable: false),
                                                      Name = table.Column<string>(nullable: true),
                                                      UploaderId = table.Column<int>(nullable: false)
                                                  },
                                         constraints: table =>
                                         {
                                             table.PrimaryKey("PK_File", x => x.Id);
                                             table.ForeignKey("FK_File_User_UploaderId",
                                                              x => x.UploaderId,
                                                              "User",
                                                              "Id",
                                                              onDelete: ReferentialAction.Cascade);
                                         });

            migrationBuilder.CreateIndex("IX_Project_ProjectIconId",
                                         "Project",
                                         "ProjectIconId");

            migrationBuilder.CreateIndex("IX_File_UploaderId",
                                         "File",
                                         "UploaderId");

            migrationBuilder.AddForeignKey("FK_Project_File_ProjectIconId",
                                           "Project",
                                           "ProjectIconId",
                                           "File",
                                           principalColumn: "Id",
                                           onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey("FK_Project_File_ProjectIconId",
                                            "Project");

            migrationBuilder.DropTable("File");

            migrationBuilder.DropIndex("IX_Project_ProjectIconId",
                                       "Project");

            migrationBuilder.DropColumn("ProjectIconId",
                                        "Project");
        }

    }

}
