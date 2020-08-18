using Microsoft.EntityFrameworkCore.Migrations;

namespace _4_Data.IdentityMigrations
{
    public partial class Updated_user_model : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "IdentityUser");

            migrationBuilder.AddColumn<string>(
                name: "ExternalPicture",
                table: "IdentityUser",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ExternalProfileUrl",
                table: "IdentityUser",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "IdentityUser",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProviderId",
                table: "IdentityUser",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SubjectId",
                table: "IdentityUser",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Username",
                table: "IdentityUser",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExternalPicture",
                table: "IdentityUser");

            migrationBuilder.DropColumn(
                name: "ExternalProfileUrl",
                table: "IdentityUser");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "IdentityUser");

            migrationBuilder.DropColumn(
                name: "ProviderId",
                table: "IdentityUser");

            migrationBuilder.DropColumn(
                name: "SubjectId",
                table: "IdentityUser");

            migrationBuilder.DropColumn(
                name: "Username",
                table: "IdentityUser");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "IdentityUser",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
