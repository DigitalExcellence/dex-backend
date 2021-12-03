using Microsoft.EntityFrameworkCore.Migrations;

namespace _4_Data.Migrations
{
    public partial class addedCascadeForTransferRequest : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectTransferRequest_User_PotentialNewOwnerId",
                table: "ProjectTransferRequest");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectTransferRequest_Project_ProjectId",
                table: "ProjectTransferRequest");

            migrationBuilder.AlterColumn<int>(
                name: "ProjectId",
                table: "ProjectTransferRequest",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "PotentialNewOwnerId",
                table: "ProjectTransferRequest",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectTransferRequest_User_PotentialNewOwnerId",
                table: "ProjectTransferRequest",
                column: "PotentialNewOwnerId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectTransferRequest_Project_ProjectId",
                table: "ProjectTransferRequest",
                column: "ProjectId",
                principalTable: "Project",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectTransferRequest_User_PotentialNewOwnerId",
                table: "ProjectTransferRequest");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectTransferRequest_Project_ProjectId",
                table: "ProjectTransferRequest");

            migrationBuilder.AlterColumn<int>(
                name: "ProjectId",
                table: "ProjectTransferRequest",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "PotentialNewOwnerId",
                table: "ProjectTransferRequest",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectTransferRequest_User_PotentialNewOwnerId",
                table: "ProjectTransferRequest",
                column: "PotentialNewOwnerId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectTransferRequest_Project_ProjectId",
                table: "ProjectTransferRequest",
                column: "ProjectId",
                principalTable: "Project",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
