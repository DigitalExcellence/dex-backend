using Microsoft.EntityFrameworkCore.Migrations;

namespace _4_Data.Migrations
{
    public partial class ProjectTransferRequestStatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "CurrentOwnerAcceptedRequest",
                table: "ProjectTransferRequest",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "PotentialNewOwnerAcceptedRequest",
                table: "ProjectTransferRequest",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CurrentOwnerAcceptedRequest",
                table: "ProjectTransferRequest");

            migrationBuilder.DropColumn(
                name: "PotentialNewOwnerAcceptedRequest",
                table: "ProjectTransferRequest");
        }
    }
}
