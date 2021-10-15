using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace _4_Data.Migrations
{
    public partial class AddUserConstraints : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(name: "Email", table: "User", maxLength: 310);
            migrationBuilder.AlterColumn<string>(name: "IdentityId", table: "User", maxLength: 255);
            migrationBuilder.AlterColumn<DateTime>(name: "AccountCreationDate", table: "User", nullable: false);
            migrationBuilder.AlterColumn<DateTime>(name: "ExpectedGraduationDate", table: "User", nullable: false);
            migrationBuilder.AddUniqueConstraint(name: "UQ_User_Email", table: "User", column: "Email");
            migrationBuilder.AddUniqueConstraint(name: "UQ_User_IdentityId", table: "User", column: "IdentityId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(name: "UQ_User_Email", table: "User");
            migrationBuilder.DropUniqueConstraint(name: "UQ_User_IdentityId", table: "User");
            migrationBuilder.AlterColumn<string>(name: "Email", table: "User", maxLength: int.MaxValue);
            migrationBuilder.AlterColumn<string>(name: "IdentityId", table: "User", maxLength: int.MaxValue);
            migrationBuilder.AlterColumn<DateTime>(name: "AccountCreationDate", table: "User", nullable: true);
            migrationBuilder.AlterColumn<DateTime>(name: "ExpectedGraduationDate", table: "User", nullable: true);
        }
    }
}
