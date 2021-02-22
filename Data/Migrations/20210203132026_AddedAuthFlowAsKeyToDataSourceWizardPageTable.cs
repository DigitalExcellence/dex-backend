using Microsoft.EntityFrameworkCore.Migrations;

namespace _4_Data.Migrations
{

    public partial class AddedAuthFlowAsKeyToDataSourceWizardPageTable : Migration
    {

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey("PK_DataSourceWizardPage",
                                            "DataSourceWizardPage");

            migrationBuilder.AddPrimaryKey("PK_DataSourceWizardPage",
                                           "DataSourceWizardPage",
                                           new[] {"DataSourceId", "WizardPageId", "AuthFlow"});
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey("PK_DataSourceWizardPage",
                                            "DataSourceWizardPage");

            migrationBuilder.AddPrimaryKey("PK_DataSourceWizardPage",
                                           "DataSourceWizardPage",
                                           new[] {"DataSourceId", "WizardPageId"});
        }

    }

}
