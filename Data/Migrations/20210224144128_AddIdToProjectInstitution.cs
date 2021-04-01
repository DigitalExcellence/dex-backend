using Microsoft.EntityFrameworkCore.Migrations;

namespace _4_Data.Migrations
{
    public partial class AddIdToProjectInstitution : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ProjectInstitution",
                table: "ProjectInstitution");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "ProjectInstitution",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProjectInstitution",
                table: "ProjectInstitution",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectInstitution_ProjectId",
                table: "ProjectInstitution",
                column: "ProjectId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ProjectInstitution",
                table: "ProjectInstitution");

            migrationBuilder.DropIndex(
                name: "IX_ProjectInstitution_ProjectId",
                table: "ProjectInstitution");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "ProjectInstitution");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProjectInstitution",
                table: "ProjectInstitution",
                columns: new[] { "ProjectId", "InstitutionId" });
        }
    }
}
