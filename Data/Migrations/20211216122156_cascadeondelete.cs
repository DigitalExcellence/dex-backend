using Microsoft.EntityFrameworkCore.Migrations;

namespace _4_Data.Migrations
{
    public partial class cascadeondelete : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectTag_Project_ProjectId",
                table: "ProjectTag");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectTag_Tag_TagId",
                table: "ProjectTag");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectTag_Project_ProjectId",
                table: "ProjectTag",
                column: "ProjectId",
                principalTable: "Project",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectTag_Tag_TagId",
                table: "ProjectTag",
                column: "TagId",
                principalTable: "Tag",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectTag_Project_ProjectId",
                table: "ProjectTag");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectTag_Tag_TagId",
                table: "ProjectTag");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectTag_Project_ProjectId",
                table: "ProjectTag",
                column: "ProjectId",
                principalTable: "Project",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectTag_Tag_TagId",
                table: "ProjectTag",
                column: "TagId",
                principalTable: "Tag",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
