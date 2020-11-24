using Microsoft.EntityFrameworkCore.Migrations;

namespace _4_Data.Migrations
{
    public partial class AddedCallToAction : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CallToActionId",
                table: "Project",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CallToAction",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    RedirectUrl = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CallToAction", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Project_CallToActionId",
                table: "Project",
                column: "CallToActionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Project_CallToAction_CallToActionId",
                table: "Project",
                column: "CallToActionId",
                principalTable: "CallToAction",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Project_CallToAction_CallToActionId",
                table: "Project");

            migrationBuilder.DropTable(
                name: "CallToAction");

            migrationBuilder.DropIndex(
                name: "IX_Project_CallToActionId",
                table: "Project");

            migrationBuilder.DropColumn(
                name: "CallToActionId",
                table: "Project");
        }
    }
}
