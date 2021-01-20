using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace _4_Data.Migrations
{
    public partial class MergedMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RoleScope_Role_RoleId",
                table: "RoleScope");

            migrationBuilder.AddColumn<DateTime>(
                name: "AccountCreationDate",
                table: "User",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ExpectedGraduationDate",
                table: "User",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "InstitutionId",
                table: "User",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsPublic",
                table: "User",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<int>(
                name: "RoleId",
                table: "RoleScope",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CallToActionId",
                table: "Project",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProjectIconId",
                table: "Project",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Highlight",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "CallToAction",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OptionValue = table.Column<string>(nullable: false),
                    Value = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CallToAction", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CallToActionOption",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<string>(nullable: false),
                    Value = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CallToActionOption", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "File",
                columns: table => new
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
                    table.ForeignKey(
                        name: "FK_File_User_UploaderId",
                        column: x => x.UploaderId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Institution",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    IdentityId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Institution", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProjectLike",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LikedProjectId = table.Column<int>(nullable: true),
                    UserId = table.Column<int>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectLike", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectLike_Project_LikedProjectId",
                        column: x => x.LikedProjectId,
                        principalTable: "Project",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProjectLike_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserProject",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProjectId = table.Column<int>(nullable: true),
                    UserId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserProject", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserProject_Project_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Project",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserProject_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserTask",
                columns: table => new
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
                    table.ForeignKey(
                        name: "FK_UserTask_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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
                name: "IX_User_InstitutionId",
                table: "User",
                column: "InstitutionId");

            migrationBuilder.CreateIndex(
                name: "IX_Project_CallToActionId",
                table: "Project",
                column: "CallToActionId");

            migrationBuilder.CreateIndex(
                name: "IX_Project_ProjectIconId",
                table: "Project",
                column: "ProjectIconId");

            migrationBuilder.CreateIndex(
                name: "IX_File_UploaderId",
                table: "File",
                column: "UploaderId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectLike_LikedProjectId",
                table: "ProjectLike",
                column: "LikedProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectLike_UserId",
                table: "ProjectLike",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserProject_ProjectId",
                table: "UserProject",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_UserProject_UserId",
                table: "UserProject",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserTask_UserId",
                table: "UserTask",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserUser_FollowedUserId",
                table: "UserUser",
                column: "FollowedUserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserUser_UserId",
                table: "UserUser",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Project_CallToAction_CallToActionId",
                table: "Project",
                column: "CallToActionId",
                principalTable: "CallToAction",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Project_File_ProjectIconId",
                table: "Project",
                column: "ProjectIconId",
                principalTable: "File",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RoleScope_Role_RoleId",
                table: "RoleScope",
                column: "RoleId",
                principalTable: "Role",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_User_Institution_InstitutionId",
                table: "User",
                column: "InstitutionId",
                principalTable: "Institution",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Project_CallToAction_CallToActionId",
                table: "Project");

            migrationBuilder.DropForeignKey(
                name: "FK_Project_File_ProjectIconId",
                table: "Project");

            migrationBuilder.DropForeignKey(
                name: "FK_RoleScope_Role_RoleId",
                table: "RoleScope");

            migrationBuilder.DropForeignKey(
                name: "FK_User_Institution_InstitutionId",
                table: "User");

            migrationBuilder.DropTable(
                name: "CallToAction");

            migrationBuilder.DropTable(
                name: "CallToActionOption");

            migrationBuilder.DropTable(
                name: "File");

            migrationBuilder.DropTable(
                name: "Institution");

            migrationBuilder.DropTable(
                name: "ProjectLike");

            migrationBuilder.DropTable(
                name: "UserProject");

            migrationBuilder.DropTable(
                name: "UserTask");

            migrationBuilder.DropTable(
                name: "UserUser");

            migrationBuilder.DropIndex(
                name: "IX_User_InstitutionId",
                table: "User");

            migrationBuilder.DropIndex(
                name: "IX_Project_CallToActionId",
                table: "Project");

            migrationBuilder.DropIndex(
                name: "IX_Project_ProjectIconId",
                table: "Project");

            migrationBuilder.DropColumn(
                name: "AccountCreationDate",
                table: "User");

            migrationBuilder.DropColumn(
                name: "ExpectedGraduationDate",
                table: "User");

            migrationBuilder.DropColumn(
                name: "InstitutionId",
                table: "User");

            migrationBuilder.DropColumn(
                name: "IsPublic",
                table: "User");

            migrationBuilder.DropColumn(
                name: "CallToActionId",
                table: "Project");

            migrationBuilder.DropColumn(
                name: "ProjectIconId",
                table: "Project");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Highlight");

            migrationBuilder.AlterColumn<int>(
                name: "RoleId",
                table: "RoleScope",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_RoleScope_Role_RoleId",
                table: "RoleScope",
                column: "RoleId",
                principalTable: "Role",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
