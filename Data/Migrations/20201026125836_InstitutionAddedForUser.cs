using Microsoft.EntityFrameworkCore.Migrations;

namespace _4_Data.Migrations
{

    public partial class InstitutionAddedForUser : Migration
    {

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>("InstitutionId",
                                            "User",
                                            nullable: true);

            migrationBuilder.CreateTable("Institution",
                                         table => new
                                                  {
                                                      Id = table.Column<int>(nullable: false)
                                                                .Annotation("SqlServer:Identity", "1, 1"),
                                                      Name = table.Column<string>(nullable: true),
                                                      Description = table.Column<string>(nullable: true)
                                                  },
                                         constraints: table => { table.PrimaryKey("PK_Institution", x => x.Id); });

            migrationBuilder.CreateIndex("IX_User_InstitutionId",
                                         "User",
                                         "InstitutionId");

            migrationBuilder.AddForeignKey("FK_User_Institution_InstitutionId",
                                           "User",
                                           "InstitutionId",
                                           "Institution",
                                           principalColumn: "Id",
                                           onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey("FK_User_Institution_InstitutionId",
                                            "User");

            migrationBuilder.DropTable("Institution");

            migrationBuilder.DropIndex("IX_User_InstitutionId",
                                       "User");

            migrationBuilder.DropColumn("InstitutionId",
                                        "User");
        }

    }

}
