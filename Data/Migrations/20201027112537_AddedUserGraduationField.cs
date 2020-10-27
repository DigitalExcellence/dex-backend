using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace _4_Data.Migrations
{
    public partial class AddedUserGraduationField : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "AccountCreationDate",
                table: "User",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ExpectedGraduationDate",
                table: "User",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccountCreationDate",
                table: "User");

            migrationBuilder.DropColumn(
                name: "ExpectedGraduationDate",
                table: "User");
        }
    }
}
