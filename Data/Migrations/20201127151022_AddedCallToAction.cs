using Microsoft.EntityFrameworkCore.Migrations;

namespace _4_Data.Migrations
{
    public partial class AddedCallToAction : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "CallToAction");

            migrationBuilder.DropColumn(
                name: "RedirectUrl",
                table: "CallToAction");

            migrationBuilder.AddColumn<string>(
                name: "OptionValue",
                table: "CallToAction",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Value",
                table: "CallToAction",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OptionValue",
                table: "CallToAction");

            migrationBuilder.DropColumn(
                name: "Value",
                table: "CallToAction");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "CallToAction",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "RedirectUrl",
                table: "CallToAction",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
