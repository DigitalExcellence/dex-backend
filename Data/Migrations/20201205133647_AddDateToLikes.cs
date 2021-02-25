using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace _4_Data.Migrations
{

    public partial class AddDateToLikes : Migration
    {

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>("Date",
                                                 "ProjectLike",
                                                 nullable: false,
                                                 defaultValue: new DateTime(
                                                     1,
                                                     1,
                                                     1,
                                                     0,
                                                     0,
                                                     0,
                                                     0,
                                                     DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn("Date",
                                        "ProjectLike");
        }

    }

}
