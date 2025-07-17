using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Portfolio.Migrations
{
    /// <inheritdoc />
    public partial class AddFeaturesTemplate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Title",
                table: "SkillsCategories",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "GithubUrl",
                table: "Projects",
                newName: "GitHubUrl");

            migrationBuilder.CreateTable(
                name: "FeaturesTemplates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nickname = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    SectionTitle = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    SectionSubtitle = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    SectionDescription = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Feature1Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Feature1Subtitle = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Feature1Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Feature1Icon = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Feature1Link = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Feature2Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Feature2Subtitle = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Feature2Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Feature2Icon = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Feature2Link = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Feature3Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Feature3Subtitle = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Feature3Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Feature3Icon = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Feature3Link = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FeaturesTemplates", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FeaturesTemplates");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "SkillsCategories",
                newName: "Title");

            migrationBuilder.RenameColumn(
                name: "GitHubUrl",
                table: "Projects",
                newName: "GithubUrl");
        }
    }
}
