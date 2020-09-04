using Microsoft.EntityFrameworkCore.Migrations;

namespace _4_Data.Migrations
{
    public partial class DecriptionAddedForHighlightTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RoleScope_Role_RoleId",
                table: "RoleScope");

            migrationBuilder.AlterColumn<int>(
                name: "RoleId",
                table: "RoleScope",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Highlight",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_RoleScope_Role_RoleId",
                table: "RoleScope",
                column: "RoleId",
                principalTable: "Role",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RoleScope_Role_RoleId",
                table: "RoleScope");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Highlight");

            migrationBuilder.AlterColumn<int>(
                name: "RoleId",
                table: "RoleScope",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_RoleScope_Role_RoleId",
                table: "RoleScope",
                column: "RoleId",
                principalTable: "Role",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
