using Microsoft.EntityFrameworkCore.Migrations;

namespace _4_Data.IdentityMigrations
{

    public partial class Updated_user_model : Migration
    {

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn("UserId",
                                        "IdentityUser");

            migrationBuilder.AddColumn<string>("ExternalPicture",
                                               "IdentityUser",
                                               nullable: true);

            migrationBuilder.AddColumn<string>("ExternalProfileUrl",
                                               "IdentityUser",
                                               nullable: true);

            migrationBuilder.AddColumn<string>("Name",
                                               "IdentityUser",
                                               nullable: true);

            migrationBuilder.AddColumn<string>("ProviderId",
                                               "IdentityUser",
                                               nullable: true);

            migrationBuilder.AddColumn<string>("SubjectId",
                                               "IdentityUser",
                                               nullable: true);

            migrationBuilder.AddColumn<string>("Username",
                                               "IdentityUser",
                                               nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn("ExternalPicture",
                                        "IdentityUser");

            migrationBuilder.DropColumn("ExternalProfileUrl",
                                        "IdentityUser");

            migrationBuilder.DropColumn("Name",
                                        "IdentityUser");

            migrationBuilder.DropColumn("ProviderId",
                                        "IdentityUser");

            migrationBuilder.DropColumn("SubjectId",
                                        "IdentityUser");

            migrationBuilder.DropColumn("Username",
                                        "IdentityUser");

            migrationBuilder.AddColumn<string>("UserId",
                                               "IdentityUser",
                                               "nvarchar(max)",
                                               nullable: true);
        }

    }

}
