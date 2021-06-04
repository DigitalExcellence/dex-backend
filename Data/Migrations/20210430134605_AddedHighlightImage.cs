using Microsoft.EntityFrameworkCore.Migrations;

namespace _4_Data.Migrations
{
    public partial class AddedHighlightImage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ImageId",
                table: "Highlight",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Highlight_ImageId",
                table: "Highlight",
                column: "ImageId");

            migrationBuilder.AddForeignKey(
                name: "FK_Highlight_File_ImageId",
                table: "Highlight",
                column: "ImageId",
                principalTable: "File",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Highlight_File_ImageId",
                table: "Highlight");

            migrationBuilder.DropIndex(
                name: "IX_Highlight_ImageId",
                table: "Highlight");

            migrationBuilder.DropColumn(
                name: "ImageId",
                table: "Highlight");
        }
    }
}
