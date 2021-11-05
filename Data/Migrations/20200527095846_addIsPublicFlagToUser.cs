using Microsoft.EntityFrameworkCore.Migrations;

namespace _4_Data.Migrations
{

    public partial class addIsPublicFlagToUser : Migration
    {

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>("IsPublic",
                                             "User",
                                             nullable: false,
                                             defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn("IsPublic",
                                        "User");
        }

    }

}
