using Microsoft.EntityFrameworkCore.Migrations;

namespace _4_Data.Migrations
{
    public partial class UpdateProjectTags : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tag_Project_ProjectId",
                table: "Tag");

            migrationBuilder.DropIndex(
                name: "IX_Tag_ProjectId",
                table: "Tag");

            migrationBuilder.DropColumn(
                name: "ProjectId",
                table: "Tag");

            migrationBuilder.CreateTable(
                name: "ProjectTag",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TagId = table.Column<int>(nullable: true),
                    ProjectId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectTag", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectTag_Project_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Project",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProjectTag_Tag_TagId",
                        column: x => x.TagId,
                        principalTable: "Tag",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProjectTag_ProjectId",
                table: "ProjectTag",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectTag_TagId",
                table: "ProjectTag",
                column: "TagId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProjectTag");

            migrationBuilder.AddColumn<int>(
                name: "ProjectId",
                table: "Tag",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tag_ProjectId",
                table: "Tag",
                column: "ProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tag_Project_ProjectId",
                table: "Tag",
                column: "ProjectId",
                principalTable: "Project",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
