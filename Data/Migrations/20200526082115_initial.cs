using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace _4_Data.Migrations
{

    public partial class initial : Migration
    {

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable("Role",
                                         table => new
                                                  {
                                                      Id = table.Column<int>(nullable: false)
                                                                .Annotation("SqlServer:Identity", "1, 1"),
                                                      Name = table.Column<string>(nullable: true)
                                                  },
                                         constraints: table => { table.PrimaryKey("PK_Role", x => x.Id); });

            migrationBuilder.CreateTable("RoleScope",
                                         table => new
                                                  {
                                                      Id = table.Column<int>(nullable: false)
                                                                .Annotation("SqlServer:Identity", "1, 1"),
                                                      Scope = table.Column<string>(nullable: false),
                                                      RoleId = table.Column<int>(nullable: false)
                                                  },
                                         constraints: table =>
                                         {
                                             table.PrimaryKey("PK_RoleScope", x => x.Id);
                                             table.ForeignKey("FK_RoleScope_Role_RoleId",
                                                              x => x.RoleId,
                                                              "Role",
                                                              "Id",
                                                              onDelete: ReferentialAction.Cascade);
                                         });

            migrationBuilder.CreateTable("User",
                                         table => new
                                                  {
                                                      Id = table.Column<int>(nullable: false)
                                                                .Annotation("SqlServer:Identity", "1, 1"),
                                                      Name = table.Column<string>(nullable: false),
                                                      RoleId = table.Column<int>(nullable: false),
                                                      Email = table.Column<string>(nullable: false),
                                                      IdentityId = table.Column<string>(nullable: false),
                                                      ProfileUrl = table.Column<string>(nullable: true)
                                                  },
                                         constraints: table =>
                                         {
                                             table.PrimaryKey("PK_User", x => x.Id);
                                             table.ForeignKey("FK_User_Role_RoleId",
                                                              x => x.RoleId,
                                                              "Role",
                                                              "Id",
                                                              onDelete: ReferentialAction.Restrict);
                                         });

            migrationBuilder.CreateTable("Project",
                                         table => new
                                                  {
                                                      Id = table.Column<int>(nullable: false)
                                                                .Annotation("SqlServer:Identity", "1, 1"),
                                                      UserId = table.Column<int>(nullable: false),
                                                      Name = table.Column<string>(nullable: false),
                                                      Description = table.Column<string>(nullable: true),
                                                      ShortDescription = table.Column<string>(nullable: false),
                                                      Uri = table.Column<string>(nullable: false),
                                                      Created = table.Column<DateTime>(nullable: false),
                                                      Updated = table.Column<DateTime>(nullable: false)
                                                  },
                                         constraints: table =>
                                         {
                                             table.PrimaryKey("PK_Project", x => x.Id);
                                             table.ForeignKey("FK_Project_User_UserId",
                                                              x => x.UserId,
                                                              "User",
                                                              "Id",
                                                              onDelete: ReferentialAction.Cascade);
                                         });

            migrationBuilder.CreateTable("Collaborators",
                                         table => new
                                                  {
                                                      Id = table.Column<int>(nullable: false)
                                                                .Annotation("SqlServer:Identity", "1, 1"),
                                                      FullName = table.Column<string>(nullable: true),
                                                      Role = table.Column<string>(nullable: true),
                                                      ProjectId = table.Column<int>(nullable: false)
                                                  },
                                         constraints: table =>
                                         {
                                             table.PrimaryKey("PK_Collaborators", x => x.Id);
                                             table.ForeignKey("FK_Collaborators_Project_ProjectId",
                                                              x => x.ProjectId,
                                                              "Project",
                                                              "Id",
                                                              onDelete: ReferentialAction.Cascade);
                                         });

            migrationBuilder.CreateTable("EmbeddedProject",
                                         table => new
                                                  {
                                                      Id = table.Column<int>(nullable: false)
                                                                .Annotation("SqlServer:Identity", "1, 1"),
                                                      UserId = table.Column<int>(nullable: false),
                                                      ProjectId = table.Column<int>(nullable: false),
                                                      Guid = table.Column<Guid>(nullable: false)
                                                  },
                                         constraints: table =>
                                         {
                                             table.PrimaryKey("PK_EmbeddedProject", x => x.Id);
                                             table.ForeignKey("FK_EmbeddedProject_Project_ProjectId",
                                                              x => x.ProjectId,
                                                              "Project",
                                                              "Id",
                                                              onDelete: ReferentialAction.Cascade);
                                             table.ForeignKey("FK_EmbeddedProject_User_UserId",
                                                              x => x.UserId,
                                                              "User",
                                                              "Id",
                                                              onDelete: ReferentialAction.NoAction);
                                         });

            migrationBuilder.CreateTable("Highlight",
                                         table => new
                                                  {
                                                      Id = table.Column<int>(nullable: false)
                                                                .Annotation("SqlServer:Identity", "1, 1"),
                                                      ProjectId = table.Column<int>(nullable: false),
                                                      StartDate = table.Column<DateTime>(nullable: true),
                                                      EndDate = table.Column<DateTime>(nullable: true)
                                                  },
                                         constraints: table =>
                                         {
                                             table.PrimaryKey("PK_Highlight", x => x.Id);
                                             table.ForeignKey("FK_Highlight_Project_ProjectId",
                                                              x => x.ProjectId,
                                                              "Project",
                                                              "Id",
                                                              onDelete: ReferentialAction.Cascade);
                                         });

            migrationBuilder.CreateIndex("IX_Collaborators_ProjectId",
                                         "Collaborators",
                                         "ProjectId");

            migrationBuilder.CreateIndex("IX_EmbeddedProject_ProjectId",
                                         "EmbeddedProject",
                                         "ProjectId");

            migrationBuilder.CreateIndex("IX_EmbeddedProject_UserId",
                                         "EmbeddedProject",
                                         "UserId");

            migrationBuilder.CreateIndex("IX_Highlight_ProjectId",
                                         "Highlight",
                                         "ProjectId");

            migrationBuilder.CreateIndex("IX_Project_UserId",
                                         "Project",
                                         "UserId");

            migrationBuilder.CreateIndex("IX_RoleScope_RoleId",
                                         "RoleScope",
                                         "RoleId");

            migrationBuilder.CreateIndex("IX_User_RoleId",
                                         "User",
                                         "RoleId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable("Collaborators");

            migrationBuilder.DropTable("EmbeddedProject");

            migrationBuilder.DropTable("Highlight");

            migrationBuilder.DropTable("RoleScope");

            migrationBuilder.DropTable("Project");

            migrationBuilder.DropTable("User");

            migrationBuilder.DropTable("Role");
        }

    }

}
