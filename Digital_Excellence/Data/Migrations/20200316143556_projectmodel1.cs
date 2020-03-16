using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class projectmodel1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IdentityId",
                table: "User");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IdentityId",
                table: "User",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
