using Microsoft.EntityFrameworkCore.Migrations;

namespace _4_Data.Migrations
{

    public partial class CreatedDataSource : Migration
    {

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable("DataSource",
                                         table => new
                                                  {
                                                      Id = table.Column<int>(nullable: false)
                                                                .Annotation("SqlServer:Identity", "1, 1"),
                                                      Guid = table.Column<string>(nullable: true),
                                                      Title = table.Column<string>(nullable: true),
                                                      Description = table.Column<string>(nullable: true),
                                                      IconId = table.Column<int>(nullable: true),
                                                      IsVisible = table.Column<bool>(nullable: false)
                                                  },
                                         constraints: table =>
                                         {
                                             table.PrimaryKey("PK_DataSource", x => x.Id);
                                             table.ForeignKey("FK_DataSource_File_IconId",
                                                              x => x.IconId,
                                                              "File",
                                                              "Id",
                                                              onDelete: ReferentialAction.Restrict);
                                         });

            migrationBuilder.CreateIndex("IX_DataSource_IconId",
                                         "DataSource",
                                         "IconId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable("DataSource");
        }

    }

}
