using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace _4_Data.Migrations
{
    public partial class AddCollaboratorLinkedUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LinkedUserId",
                table: "Collaborators",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CollaboratorLinkedUser",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(nullable: true),
                    Status = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CollaboratorLinkedUser", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CollaboratorLinkedUser_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Collaborators_LinkedUserId",
                table: "Collaborators",
                column: "LinkedUserId");

            migrationBuilder.CreateIndex(
                name: "IX_CollaboratorLinkedUser_UserId",
                table: "CollaboratorLinkedUser",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Collaborators_CollaboratorLinkedUser_LinkedUserId",
                table: "Collaborators",
                column: "LinkedUserId",
                principalTable: "CollaboratorLinkedUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.CreateTable(
                name: "CollaboratorLinkRequest",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RequestHash = table.Column<string>(nullable: true),
                    CollaboratorId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CollaboratorLinkRequest", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CollaboratorLinkRequest_Collaborators_CollaboratorId",
                        column: x => x.CollaboratorId,
                        principalTable: "Collaborators",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Collaborators_CollaboratorLinkedUser_LinkedUserId",
                table: "Collaborators");

            migrationBuilder.DropTable(
                name: "CollaboratorLinkedUser");

            migrationBuilder.DropIndex(
                name: "IX_Collaborators_LinkedUserId",
                table: "Collaborators");

            migrationBuilder.DropColumn(
                name: "LinkedUserId",
                table: "Collaborators");

            migrationBuilder.DropTable(
               name: "CollaboratorLinkRequest");
        }
    }
}
