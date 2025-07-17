using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Portfolio.Migrations
{
    /// <inheritdoc />
    public partial class AddFeaturesSection : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FeaturesSections",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SectionTitle = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Feature1Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Feature1Subtitle = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Feature2Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Feature2Subtitle = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Feature3Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Feature3Subtitle = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FeaturesSections", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FeaturesSections");
        }
    }
}
