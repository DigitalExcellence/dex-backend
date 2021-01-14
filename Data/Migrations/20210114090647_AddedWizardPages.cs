using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace _4_Data.Migrations
{
    public partial class AddedWizardPages : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WizardPage",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    UpdatedAt = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WizardPage", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DataSourceWizardPage",
                columns: table => new
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
                    table.ForeignKey(
                        name: "FK_DataSourceWizardPage_DataSource_DataSourceId",
                        column: x => x.DataSourceId,
                        principalTable: "DataSource",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DataSourceWizardPage_WizardPage_WizardPageId",
                        column: x => x.WizardPageId,
                        principalTable: "WizardPage",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DataSourceWizardPage_DataSourceId",
                table: "DataSourceWizardPage",
                column: "DataSourceId");

            migrationBuilder.CreateIndex(
                name: "IX_DataSourceWizardPage_WizardPageId",
                table: "DataSourceWizardPage",
                column: "WizardPageId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DataSourceWizardPage");

            migrationBuilder.DropTable(
                name: "WizardPage");
        }
    }
}
