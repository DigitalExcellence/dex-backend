using Microsoft.EntityFrameworkCore.Migrations;

namespace _4_Data.Migrations
{

    public partial class DecriptionAddedForHighlightTable : Migration
    {

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey("FK_RoleScope_Role_RoleId",
                                            "RoleScope");

            migrationBuilder.AlterColumn<int>("RoleId",
                                              "RoleScope",
                                              nullable: false,
                                              oldClrType: typeof(int),
                                              oldType: "int",
                                              oldNullable: true);

            migrationBuilder.AddColumn<string>("Description",
                                               "Highlight",
                                               nullable: false,
                                               defaultValue: "");

            migrationBuilder.AddForeignKey("FK_RoleScope_Role_RoleId",
                                           "RoleScope",
                                           "RoleId",
                                           "Role",
                                           principalColumn: "Id",
                                           onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey("FK_RoleScope_Role_RoleId",
                                            "RoleScope");

            migrationBuilder.DropColumn("Description",
                                        "Highlight");

            migrationBuilder.AlterColumn<int>("RoleId",
                                              "RoleScope",
                                              "int",
                                              nullable: true,
                                              oldClrType: typeof(int));

            migrationBuilder.AddForeignKey("FK_RoleScope_Role_RoleId",
                                           "RoleScope",
                                           "RoleId",
                                           "Role",
                                           principalColumn: "Id",
                                           onDelete: ReferentialAction.Restrict);
        }

    }

}
