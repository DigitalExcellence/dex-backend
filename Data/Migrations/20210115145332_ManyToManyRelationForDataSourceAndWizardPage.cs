using Microsoft.EntityFrameworkCore.Migrations;

namespace _4_Data.Migrations
{

    public partial class ManyToManyRelationForDataSourceAndWizardPage : Migration
    {

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey("FK_DataSourceWizardPage_DataSource_DataSourceId",
                                            "DataSourceWizardPage");

            migrationBuilder.DropForeignKey("FK_DataSourceWizardPage_WizardPage_WizardPageId",
                                            "DataSourceWizardPage");

            migrationBuilder.DropPrimaryKey("PK_DataSourceWizardPage",
                                            "DataSourceWizardPage");

            migrationBuilder.DropIndex("IX_DataSourceWizardPage_DataSourceId",
                                       "DataSourceWizardPage");

            migrationBuilder.DropColumn("Id",
                                        "DataSourceWizardPage");

            migrationBuilder.AlterColumn<int>("WizardPageId",
                                              "DataSourceWizardPage",
                                              nullable: false,
                                              oldClrType: typeof(int),
                                              oldType: "int",
                                              oldNullable: true);

            migrationBuilder.AlterColumn<int>("DataSourceId",
                                              "DataSourceWizardPage",
                                              nullable: false,
                                              oldClrType: typeof(int),
                                              oldType: "int",
                                              oldNullable: true);

            migrationBuilder.AddPrimaryKey("PK_DataSourceWizardPage",
                                           "DataSourceWizardPage",
                                           new[] {"DataSourceId", "WizardPageId"});

            migrationBuilder.AddForeignKey("FK_DataSourceWizardPage_DataSource_DataSourceId",
                                           "DataSourceWizardPage",
                                           "DataSourceId",
                                           "DataSource",
                                           principalColumn: "Id",
                                           onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey("FK_DataSourceWizardPage_WizardPage_WizardPageId",
                                           "DataSourceWizardPage",
                                           "WizardPageId",
                                           "WizardPage",
                                           principalColumn: "Id",
                                           onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey("FK_DataSourceWizardPage_DataSource_DataSourceId",
                                            "DataSourceWizardPage");

            migrationBuilder.DropForeignKey("FK_DataSourceWizardPage_WizardPage_WizardPageId",
                                            "DataSourceWizardPage");

            migrationBuilder.DropPrimaryKey("PK_DataSourceWizardPage",
                                            "DataSourceWizardPage");

            migrationBuilder.AlterColumn<int>("WizardPageId",
                                              "DataSourceWizardPage",
                                              "int",
                                              nullable: true,
                                              oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>("DataSourceId",
                                              "DataSourceWizardPage",
                                              "int",
                                              nullable: true,
                                              oldClrType: typeof(int));

            migrationBuilder.AddColumn<int>("Id",
                                            "DataSourceWizardPage",
                                            "int",
                                            nullable: false,
                                            defaultValue: 0)
                            .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey("PK_DataSourceWizardPage",
                                           "DataSourceWizardPage",
                                           "Id");

            migrationBuilder.CreateIndex("IX_DataSourceWizardPage_DataSourceId",
                                         "DataSourceWizardPage",
                                         "DataSourceId");

            migrationBuilder.AddForeignKey("FK_DataSourceWizardPage_DataSource_DataSourceId",
                                           "DataSourceWizardPage",
                                           "DataSourceId",
                                           "DataSource",
                                           principalColumn: "Id",
                                           onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey("FK_DataSourceWizardPage_WizardPage_WizardPageId",
                                           "DataSourceWizardPage",
                                           "WizardPageId",
                                           "WizardPage",
                                           principalColumn: "Id",
                                           onDelete: ReferentialAction.Restrict);
        }

    }

}
