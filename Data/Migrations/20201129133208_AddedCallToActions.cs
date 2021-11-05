using Microsoft.EntityFrameworkCore.Migrations;

namespace _4_Data.Migrations
{

    public partial class AddedCallToActions : Migration
    {

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>("CallToActionId",
                                            "Project",
                                            nullable: true);

            migrationBuilder.CreateTable("CallToAction",
                                         table => new
                                                  {
                                                      Id = table.Column<int>(nullable: false)
                                                                .Annotation("SqlServer:Identity", "1, 1"),
                                                      OptionValue = table.Column<string>(nullable: false),
                                                      Value = table.Column<string>(nullable: false)
                                                  },
                                         constraints: table => { table.PrimaryKey("PK_CallToAction", x => x.Id); });

            migrationBuilder.CreateTable("CallToActionOption",
                                         table => new
                                                  {
                                                      Id = table.Column<int>(nullable: false)
                                                                .Annotation("SqlServer:Identity", "1, 1"),
                                                      Type = table.Column<string>(nullable: false),
                                                      Value = table.Column<string>(nullable: false)
                                                  },
                                         constraints: table =>
                                         {
                                             table.PrimaryKey("PK_CallToActionOption", x => x.Id);
                                         });

            migrationBuilder.CreateIndex("IX_Project_CallToActionId",
                                         "Project",
                                         "CallToActionId");

            migrationBuilder.AddForeignKey("FK_Project_CallToAction_CallToActionId",
                                           "Project",
                                           "CallToActionId",
                                           "CallToAction",
                                           principalColumn: "Id",
                                           onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey("FK_Project_CallToAction_CallToActionId",
                                            "Project");

            migrationBuilder.DropTable("CallToAction");

            migrationBuilder.DropTable("CallToActionOption");

            migrationBuilder.DropIndex("IX_Project_CallToActionId",
                                       "Project");

            migrationBuilder.DropColumn("CallToActionId",
                                        "Project");
        }

    }

}
