﻿using Microsoft.EntityFrameworkCore.Migrations;

namespace _4_Data.Migrations
{

    public partial class AddedIntitutePrivateToProject : Migration
    {

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>("InstitutePrivate",
                                             "Project",
                                             nullable: false,
                                             defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn("InstitutePrivate",
                                        "Project");
        }

    }

}
