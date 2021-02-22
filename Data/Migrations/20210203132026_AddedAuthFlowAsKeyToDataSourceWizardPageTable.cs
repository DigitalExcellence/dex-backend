using Microsoft.EntityFrameworkCore.Migrations;

namespace _4_Data.Migrations
{
    public partial class AddedAuthFlowAsKeyToDataSourceWizardPageTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_DataSourceWizardPage",
                table: "DataSourceWizardPage");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DataSourceWizardPage",
                table: "DataSourceWizardPage",
                columns: new[] { "DataSourceId", "WizardPageId", "AuthFlow" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_DataSourceWizardPage",
                table: "DataSourceWizardPage");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DataSourceWizardPage",
                table: "DataSourceWizardPage",
                columns: new[] { "DataSourceId", "WizardPageId" });
        }
    }
}
