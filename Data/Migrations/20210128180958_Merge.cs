using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace _4_Data.Migrations
{

    public partial class Merge : Migration
    {

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>("AccountCreationDate",
                                                 "User",
                                                 nullable: true);

            migrationBuilder.AddColumn<DateTime>("ExpectedGraduationDate",
                                                 "User",
                                                 nullable: true);

            migrationBuilder.CreateTable("UserTask",
                                         table => new
                                                  {
                                                      Id = table.Column<int>(nullable: false)
                                                                .Annotation("SqlServer:Identity", "1, 1"),
                                                      UserId = table.Column<int>(nullable: true),
                                                      Status = table.Column<int>(nullable: false),
                                                      Type = table.Column<int>(nullable: false)
                                                  },
                                         constraints: table =>
                                         {
                                             table.PrimaryKey("PK_UserTask", x => x.Id);
                                             table.ForeignKey("FK_UserTask_User_UserId",
                                                              x => x.UserId,
                                                              "User",
                                                              "Id",
                                                              onDelete: ReferentialAction.Cascade);
                                         });

            migrationBuilder.CreateIndex("IX_UserTask_UserId",
                                         "UserTask",
                                         "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable("UserTask");

            migrationBuilder.DropColumn("AccountCreationDate",
                                        "User");

            migrationBuilder.DropColumn("ExpectedGraduationDate",
                                        "User");
        }

    }

}
