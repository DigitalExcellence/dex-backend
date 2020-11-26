using Microsoft.EntityFrameworkCore.Migrations;

namespace _4_Data.Migrations
{
    public partial class AddedCallToActionOptions : Migration
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
                    Name = table.Column<string>(nullable: false),
                    RedirectUrl = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CallToAction", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CallToActionOptionType",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CallToActionOptionType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CallToActionOption",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TypeId = table.Column<int>(nullable: false),
                    Value = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CallToActionOption", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CallToActionOption_CallToActionOptionType_TypeId",
                        column: x => x.TypeId,
                        principalTable: "CallToActionOptionType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Project_CallToActionId",
                table: "Project",
                column: "CallToActionId");

            migrationBuilder.CreateIndex(
                name: "IX_CallToActionOption_TypeId",
                table: "CallToActionOption",
                column: "TypeId");

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

            migrationBuilder.DropTable(
                name: "CallToActionOption");

            migrationBuilder.DropTable(
                name: "CallToActionOptionType");

            migrationBuilder.DropIndex(
                name: "IX_Project_CallToActionId",
                table: "Project");

            migrationBuilder.DropColumn(
                name: "CallToActionId",
                table: "Project");
        }
    }
}
