using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace _4_Data.Migrations
{

    public partial class AddedWizardPages : Migration
    {

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable("WizardPage",
                                         table => new
                                                  {
                                                      Id = table.Column<int>(nullable: false)
                                                                .Annotation("SqlServer:Identity", "1, 1"),
                                                      Name = table.Column<string>(nullable: true),
                                                      Description = table.Column<string>(nullable: true),
                                                      CreatedAt = table.Column<DateTime>(nullable: false),
                                                      UpdatedAt = table.Column<DateTime>(nullable: false)
                                                  },
                                         constraints: table => { table.PrimaryKey("PK_WizardPage", x => x.Id); });

            migrationBuilder.CreateTable("DataSourceWizardPage",
                                         table => new
                                                  {
                                                      Id = table.Column<int>(nullable: false)
                                                                .Annotation("SqlServer:Identity", "1, 1"),
                                                      DataSourceId = table.Column<int>(nullable: true),
                                                      WizardPageId = table.Column<int>(nullable: true),
                                                      AuthFlow = table.Column<bool>(nullable: false),
                                                      OrderIndex = table.Column<int>(nullable: false)
                                                  },
                                         constraints: table =>
                                         {
                                             table.PrimaryKey("PK_DataSourceWizardPage", x => x.Id);
                                             table.ForeignKey("FK_DataSourceWizardPage_DataSource_DataSourceId",
                                                              x => x.DataSourceId,
                                                              "DataSource",
                                                              "Id",
                                                              onDelete: ReferentialAction.Restrict);
                                             table.ForeignKey("FK_DataSourceWizardPage_WizardPage_WizardPageId",
                                                              x => x.WizardPageId,
                                                              "WizardPage",
                                                              "Id",
                                                              onDelete: ReferentialAction.Restrict);
                                         });

            migrationBuilder.CreateIndex("IX_DataSourceWizardPage_DataSourceId",
                                         "DataSourceWizardPage",
                                         "DataSourceId");

            migrationBuilder.CreateIndex("IX_DataSourceWizardPage_WizardPageId",
                                         "DataSourceWizardPage",
                                         "WizardPageId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable("DataSourceWizardPage");

            migrationBuilder.DropTable("WizardPage");
        }

    }

}
