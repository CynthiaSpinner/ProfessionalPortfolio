using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Portfolio.Migrations
{
    /// <inheritdoc />
    public partial class UpdateFeaturesSectionStructure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DisplayOrder",
                table: "FeaturesSections",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Feature1Description",
                table: "FeaturesSections",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Feature1Icon",
                table: "FeaturesSections",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Feature1Link",
                table: "FeaturesSections",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Feature2Description",
                table: "FeaturesSections",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Feature2Icon",
                table: "FeaturesSections",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Feature2Link",
                table: "FeaturesSections",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Feature3Description",
                table: "FeaturesSections",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Feature3Icon",
                table: "FeaturesSections",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Feature3Link",
                table: "FeaturesSections",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "FeaturesSections",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "SectionSubtitle",
                table: "FeaturesSections",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DisplayOrder",
                table: "FeaturesSections");

            migrationBuilder.DropColumn(
                name: "Feature1Description",
                table: "FeaturesSections");

            migrationBuilder.DropColumn(
                name: "Feature1Icon",
                table: "FeaturesSections");

            migrationBuilder.DropColumn(
                name: "Feature1Link",
                table: "FeaturesSections");

            migrationBuilder.DropColumn(
                name: "Feature2Description",
                table: "FeaturesSections");

            migrationBuilder.DropColumn(
                name: "Feature2Icon",
                table: "FeaturesSections");

            migrationBuilder.DropColumn(
                name: "Feature2Link",
                table: "FeaturesSections");

            migrationBuilder.DropColumn(
                name: "Feature3Description",
                table: "FeaturesSections");

            migrationBuilder.DropColumn(
                name: "Feature3Icon",
                table: "FeaturesSections");

            migrationBuilder.DropColumn(
                name: "Feature3Link",
                table: "FeaturesSections");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "FeaturesSections");

            migrationBuilder.DropColumn(
                name: "SectionSubtitle",
                table: "FeaturesSections");
        }
    }
}
