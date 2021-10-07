using Microsoft.EntityFrameworkCore.Migrations;

namespace _4_Data.Migrations
{
    public partial class ProjectTransferRequest : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProjectTransferRequest",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProjectId = table.Column<int>(nullable: true),
                    PotentialNewOwnerId = table.Column<int>(nullable: true),
                    Status = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectTransferRequest", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectTransferRequest_User_PotentialNewOwnerId",
                        column: x => x.PotentialNewOwnerId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProjectTransferRequest_Project_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Project",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProjectTransferRequest_PotentialNewOwnerId",
                table: "ProjectTransferRequest",
                column: "PotentialNewOwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectTransferRequest_ProjectId",
                table: "ProjectTransferRequest",
                column: "ProjectId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProjectTransferRequest");
        }
    }
}
