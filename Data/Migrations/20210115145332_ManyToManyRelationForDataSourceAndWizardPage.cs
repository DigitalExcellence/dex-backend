using Microsoft.EntityFrameworkCore.Migrations;

namespace _4_Data.Migrations
{
    public partial class ManyToManyRelationForDataSourceAndWizardPage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DataSourceWizardPage_DataSource_DataSourceId",
                table: "DataSourceWizardPage");

            migrationBuilder.DropForeignKey(
                name: "FK_DataSourceWizardPage_WizardPage_WizardPageId",
                table: "DataSourceWizardPage");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DataSourceWizardPage",
                table: "DataSourceWizardPage");

            migrationBuilder.DropIndex(
                name: "IX_DataSourceWizardPage_DataSourceId",
                table: "DataSourceWizardPage");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "DataSourceWizardPage");

            migrationBuilder.AlterColumn<int>(
                name: "WizardPageId",
                table: "DataSourceWizardPage",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "DataSourceId",
                table: "DataSourceWizardPage",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_DataSourceWizardPage",
                table: "DataSourceWizardPage",
                columns: new[] { "DataSourceId", "WizardPageId" });

            migrationBuilder.AddForeignKey(
                name: "FK_DataSourceWizardPage_DataSource_DataSourceId",
                table: "DataSourceWizardPage",
                column: "DataSourceId",
                principalTable: "DataSource",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DataSourceWizardPage_WizardPage_WizardPageId",
                table: "DataSourceWizardPage",
                column: "WizardPageId",
                principalTable: "WizardPage",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DataSourceWizardPage_DataSource_DataSourceId",
                table: "DataSourceWizardPage");

            migrationBuilder.DropForeignKey(
                name: "FK_DataSourceWizardPage_WizardPage_WizardPageId",
                table: "DataSourceWizardPage");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DataSourceWizardPage",
                table: "DataSourceWizardPage");

            migrationBuilder.AlterColumn<int>(
                name: "WizardPageId",
                table: "DataSourceWizardPage",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "DataSourceId",
                table: "DataSourceWizardPage",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "DataSourceWizardPage",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DataSourceWizardPage",
                table: "DataSourceWizardPage",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_DataSourceWizardPage_DataSourceId",
                table: "DataSourceWizardPage",
                column: "DataSourceId");

            migrationBuilder.AddForeignKey(
                name: "FK_DataSourceWizardPage_DataSource_DataSourceId",
                table: "DataSourceWizardPage",
                column: "DataSourceId",
                principalTable: "DataSource",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DataSourceWizardPage_WizardPage_WizardPageId",
                table: "DataSourceWizardPage",
                column: "WizardPageId",
                principalTable: "WizardPage",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
