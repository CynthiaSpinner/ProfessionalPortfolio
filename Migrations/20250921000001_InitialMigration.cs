using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Portfolio.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Admins",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Username = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastLoginAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Admins", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Abouts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: false),
                    Biography = table.Column<string>(type: "text", nullable: false),
                    ProfileImageUrl = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    LinkedInUrl = table.Column<string>(type: "text", nullable: false),
                    GithubUrl = table.Column<string>(type: "text", nullable: false),
                    InterestsJson = table.Column<string>(type: "jsonb", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Abouts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WorkExperiences",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Company = table.Column<string>(type: "text", nullable: false),
                    Position = table.Column<string>(type: "text", nullable: false),
                    StartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EndDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: false),
                    AchievementsJson = table.Column<string>(type: "jsonb", nullable: false),
                    AboutId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkExperiences", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkExperiences_Abouts_AboutId",
                        column: x => x.AboutId,
                        principalTable: "Abouts",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Educations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Institution = table.Column<string>(type: "text", nullable: false),
                    Degree = table.Column<string>(type: "text", nullable: false),
                    Field = table.Column<string>(type: "text", nullable: false),
                    StartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EndDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: false),
                    AboutId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Educations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Educations_Abouts_AboutId",
                        column: x => x.AboutId,
                        principalTable: "Abouts",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "HomePages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    HeaderTitle = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    HeaderSubtitle = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    HeaderDescription = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    HeaderBackgroundImageUrl = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    HeaderBackgroundVideoUrl = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    HeaderBackgroundStyle = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    HeaderPrimaryButtonText = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    HeaderPrimaryButtonUrl = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    HeaderSecondaryButtonText = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    HeaderSecondaryButtonUrl = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    ShowHeaderPrimaryButton = table.Column<bool>(type: "boolean", nullable: false),
                    ShowHeaderSecondaryButton = table.Column<bool>(type: "boolean", nullable: false),
                    HeaderOverlayColor = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    HeaderOverlayOpacity = table.Column<float>(type: "real", nullable: true),
                    HeaderTextColor = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    HeaderButtonColor = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    HeaderButtonTextColor = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    SkillsSectionTitle = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    SkillsSectionSubtitle = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    SkillsCategoriesJson = table.Column<string>(type: "jsonb", nullable: false),
                    FeaturedProjectsTitle = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    FeaturedProjectsSubtitle = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    FeaturedProjectsJson = table.Column<string>(type: "jsonb", nullable: false),
                    CTATitle = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    CTASubtitle = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    CTAButtonText = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    CTAButtonUrl = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    CTABackgroundImageUrl = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    CTATextColor = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    DisplayOrder = table.Column<int>(type: "integer", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HomePages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Projects",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Title = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    ImageUrl = table.Column<string>(type: "text", nullable: false),
                    ProjectUrl = table.Column<string>(type: "text", nullable: false),
                    GitHubUrl = table.Column<string>(type: "text", nullable: false),
                    TechnologiesJson = table.Column<string>(type: "jsonb", nullable: false),
                    CompletionDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Projects", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SkillsCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    ImagePath = table.Column<string>(type: "text", nullable: false),
                    SkillsJson = table.Column<string>(type: "jsonb", nullable: false),
                    DisplayOrder = table.Column<int>(type: "integer", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SkillsCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Videos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Title = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    FilePath = table.Column<string>(type: "text", nullable: false),
                    ThumbnailPath = table.Column<string>(type: "text", nullable: false),
                    FileType = table.Column<string>(type: "text", nullable: false),
                    FileSize = table.Column<long>(type: "bigint", nullable: false),
                    Duration = table.Column<int>(type: "integer", nullable: false),
                    UploadDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    DisplayOrder = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Videos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HeroTemplates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nickname = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    HeaderTitle = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    HeaderSubtitle = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    HeaderDescription = table.Column<string>(type: "text", nullable: false),
                    HeaderBackgroundImageUrl = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    HeaderBackgroundVideoUrl = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    HeaderPrimaryButtonText = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    HeaderPrimaryButtonUrl = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    HeaderOverlayColor = table.Column<string>(type: "character varying(7)", maxLength: 7, nullable: false),
                    HeaderOverlayOpacity = table.Column<float>(type: "real", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CTABackgroundImageUrl = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    CTAButtonText = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    CTAButtonUrl = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    CTASubtitle = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    CTATextColor = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    CTATitle = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    FeaturedProjectsJson = table.Column<string>(type: "text", nullable: true),
                    FeaturedProjectsSubtitle = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    FeaturedProjectsTitle = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    HeaderBackgroundStyle = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    HeaderButtonColor = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    HeaderButtonTextColor = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    HeaderSecondaryButtonText = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    HeaderSecondaryButtonUrl = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    HeaderTextColor = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    ShowHeaderPrimaryButton = table.Column<bool>(type: "boolean", nullable: true),
                    ShowHeaderSecondaryButton = table.Column<bool>(type: "boolean", nullable: true),
                    SkillsCategoriesJson = table.Column<string>(type: "text", nullable: true),
                    SkillsSectionSubtitle = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    SkillsSectionTitle = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Template = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HeroTemplates", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FeaturesSections",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SectionTitle = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    SectionSubtitle = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Feature1Title = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Feature1Subtitle = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Feature1Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Feature1Icon = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Feature1Link = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    Feature2Title = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Feature2Subtitle = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Feature2Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Feature2Icon = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Feature2Link = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    Feature3Title = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Feature3Subtitle = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Feature3Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Feature3Icon = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Feature3Link = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    DisplayOrder = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FeaturesSections", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FeaturesTemplates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nickname = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    SectionTitle = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    SectionSubtitle = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    SectionDescription = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    Feature1Title = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Feature1Subtitle = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Feature1Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    Feature1Icon = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Feature1Link = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Feature2Title = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Feature2Subtitle = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Feature2Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    Feature2Icon = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Feature2Link = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Feature3Title = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Feature3Subtitle = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Feature3Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    Feature3Icon = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Feature3Link = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FeaturesTemplates", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CTASections",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Title = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Subtitle = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    ButtonText = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    ButtonLink = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CTASections", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CTATemplates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nickname = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Title = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Subtitle = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    ButtonText = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    ButtonLink = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CTATemplates", x => x.Id);
                });

            // Create indexes
            migrationBuilder.CreateIndex(
                name: "IX_Admins_Email",
                table: "Admins",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Admins_Username",
                table: "Admins",
                column: "Username",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_HomePages_IsActive",
                table: "HomePages",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_HomePages_DisplayOrder",
                table: "HomePages",
                column: "DisplayOrder");

            migrationBuilder.CreateIndex(
                name: "IX_SkillsCategories_IsActive",
                table: "SkillsCategories",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_SkillsCategories_DisplayOrder",
                table: "SkillsCategories",
                column: "DisplayOrder");

            migrationBuilder.CreateIndex(
                name: "IX_WorkExperiences_AboutId",
                table: "WorkExperiences",
                column: "AboutId");

            migrationBuilder.CreateIndex(
                name: "IX_Educations_AboutId",
                table: "Educations",
                column: "AboutId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "Admins");
            migrationBuilder.DropTable(name: "Abouts");
            migrationBuilder.DropTable(name: "WorkExperiences");
            migrationBuilder.DropTable(name: "Educations");
            migrationBuilder.DropTable(name: "HomePages");
            migrationBuilder.DropTable(name: "Projects");
            migrationBuilder.DropTable(name: "SkillsCategories");
            migrationBuilder.DropTable(name: "Videos");
            migrationBuilder.DropTable(name: "HeroTemplates");
            migrationBuilder.DropTable(name: "FeaturesSections");
            migrationBuilder.DropTable(name: "FeaturesTemplates");
            migrationBuilder.DropTable(name: "CTASections");
            migrationBuilder.DropTable(name: "CTATemplates");
        }
    }
}
