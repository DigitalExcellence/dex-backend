using Microsoft.EntityFrameworkCore.Migrations;

namespace _4_Data.IdentityMigrations
{
    public partial class Updated_user_model2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ExternalSubjectId",
                table: "IdentityUser",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExternalSubjectId",
                table: "IdentityUser");
        }
    }
}
