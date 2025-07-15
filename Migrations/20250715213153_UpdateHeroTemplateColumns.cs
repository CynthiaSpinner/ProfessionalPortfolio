using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Portfolio.Migrations
{
    /// <inheritdoc />
    public partial class UpdateHeroTemplateColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<float>(
                name: "HeaderOverlayOpacity",
                table: "HeroTemplates",
                type: "real",
                nullable: true,
                oldClrType: typeof(float),
                oldType: "real");

            migrationBuilder.AddColumn<string>(
                name: "CTABackgroundImageUrl",
                table: "HeroTemplates",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CTAButtonText",
                table: "HeroTemplates",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CTAButtonUrl",
                table: "HeroTemplates",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CTASubtitle",
                table: "HeroTemplates",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CTATextColor",
                table: "HeroTemplates",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CTATitle",
                table: "HeroTemplates",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FeaturedProjectsJson",
                table: "HeroTemplates",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FeaturedProjectsSubtitle",
                table: "HeroTemplates",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FeaturedProjectsTitle",
                table: "HeroTemplates",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HeaderBackgroundStyle",
                table: "HeroTemplates",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HeaderButtonColor",
                table: "HeroTemplates",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HeaderButtonTextColor",
                table: "HeroTemplates",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HeaderSecondaryButtonText",
                table: "HeroTemplates",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HeaderSecondaryButtonUrl",
                table: "HeroTemplates",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HeaderTextColor",
                table: "HeroTemplates",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "HeroTemplates",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "HeroTemplates",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "ShowHeaderPrimaryButton",
                table: "HeroTemplates",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "ShowHeaderSecondaryButton",
                table: "HeroTemplates",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SkillsCategoriesJson",
                table: "HeroTemplates",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SkillsSectionSubtitle",
                table: "HeroTemplates",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SkillsSectionTitle",
                table: "HeroTemplates",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Template",
                table: "HeroTemplates",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CTABackgroundImageUrl",
                table: "HeroTemplates");

            migrationBuilder.DropColumn(
                name: "CTAButtonText",
                table: "HeroTemplates");

            migrationBuilder.DropColumn(
                name: "CTAButtonUrl",
                table: "HeroTemplates");

            migrationBuilder.DropColumn(
                name: "CTASubtitle",
                table: "HeroTemplates");

            migrationBuilder.DropColumn(
                name: "CTATextColor",
                table: "HeroTemplates");

            migrationBuilder.DropColumn(
                name: "CTATitle",
                table: "HeroTemplates");

            migrationBuilder.DropColumn(
                name: "FeaturedProjectsJson",
                table: "HeroTemplates");

            migrationBuilder.DropColumn(
                name: "FeaturedProjectsSubtitle",
                table: "HeroTemplates");

            migrationBuilder.DropColumn(
                name: "FeaturedProjectsTitle",
                table: "HeroTemplates");

            migrationBuilder.DropColumn(
                name: "HeaderBackgroundStyle",
                table: "HeroTemplates");

            migrationBuilder.DropColumn(
                name: "HeaderButtonColor",
                table: "HeroTemplates");

            migrationBuilder.DropColumn(
                name: "HeaderButtonTextColor",
                table: "HeroTemplates");

            migrationBuilder.DropColumn(
                name: "HeaderSecondaryButtonText",
                table: "HeroTemplates");

            migrationBuilder.DropColumn(
                name: "HeaderSecondaryButtonUrl",
                table: "HeroTemplates");

            migrationBuilder.DropColumn(
                name: "HeaderTextColor",
                table: "HeroTemplates");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "HeroTemplates");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "HeroTemplates");

            migrationBuilder.DropColumn(
                name: "ShowHeaderPrimaryButton",
                table: "HeroTemplates");

            migrationBuilder.DropColumn(
                name: "ShowHeaderSecondaryButton",
                table: "HeroTemplates");

            migrationBuilder.DropColumn(
                name: "SkillsCategoriesJson",
                table: "HeroTemplates");

            migrationBuilder.DropColumn(
                name: "SkillsSectionSubtitle",
                table: "HeroTemplates");

            migrationBuilder.DropColumn(
                name: "SkillsSectionTitle",
                table: "HeroTemplates");

            migrationBuilder.DropColumn(
                name: "Template",
                table: "HeroTemplates");

            migrationBuilder.AlterColumn<float>(
                name: "HeaderOverlayOpacity",
                table: "HeroTemplates",
                type: "real",
                nullable: false,
                defaultValue: 0f,
                oldClrType: typeof(float),
                oldType: "real",
                oldNullable: true);
        }
    }
}
