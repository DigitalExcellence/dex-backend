using Microsoft.EntityFrameworkCore.Migrations;

namespace _4_Data.Migrations
{
    public partial class AddedActivityAlgorithmMultiplier : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ActivityAlgorithmMultiplier",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LikeDataMultiplier = table.Column<int>(nullable: false),
                    RecentCreatedDataMultiplier = table.Column<int>(nullable: false),
                    AverageLikeDateMultiplier = table.Column<int>(nullable: false),
                    UpdatedTimeMultiplier = table.Column<int>(nullable: false),
                    InstitutionMultiplier = table.Column<int>(nullable: false),
                    ConnectedCollaboratorsMultiplier = table.Column<int>(nullable: false),
                    MetaDataMultiplier = table.Column<int>(nullable: false),
                    RepoScoreMultiplier = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActivityAlgorithmMultiplier", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ActivityAlgorithmMultiplier");
        }
    }
}
