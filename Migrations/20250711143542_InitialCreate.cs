using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Portfolio.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Abouts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Biography = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProfileImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LinkedInUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GithubUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    InterestsJson = table.Column<string>(type: "json", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Abouts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Admins",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastLoginAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Admins", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HeroTemplates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nickname = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    HeaderTitle = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    HeaderSubtitle = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    HeaderDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HeaderBackgroundImageUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    HeaderBackgroundVideoUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    HeaderPrimaryButtonText = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    HeaderPrimaryButtonUrl = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    HeaderOverlayColor = table.Column<string>(type: "nvarchar(7)", maxLength: 7, nullable: false),
                    HeaderOverlayOpacity = table.Column<float>(type: "real", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HeroTemplates", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HomePages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HeaderTitle = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    HeaderSubtitle = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    HeaderDescription = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    HeaderBackgroundImageUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    HeaderBackgroundVideoUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    HeaderBackgroundStyle = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    HeaderPrimaryButtonText = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    HeaderPrimaryButtonUrl = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    HeaderSecondaryButtonText = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    HeaderSecondaryButtonUrl = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ShowHeaderPrimaryButton = table.Column<bool>(type: "bit", nullable: false),
                    ShowHeaderSecondaryButton = table.Column<bool>(type: "bit", nullable: false),
                    HeaderOverlayColor = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    HeaderOverlayOpacity = table.Column<float>(type: "real", nullable: true),
                    HeaderTextColor = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    HeaderButtonColor = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    HeaderButtonTextColor = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    SkillsSectionTitle = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    SkillsSectionSubtitle = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    SkillsCategoriesJson = table.Column<string>(type: "json", nullable: false),
                    FeaturedProjectsTitle = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    FeaturedProjectsSubtitle = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    FeaturedProjectsJson = table.Column<string>(type: "json", nullable: false),
                    CTATitle = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CTASubtitle = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CTAButtonText = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CTAButtonUrl = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    CTABackgroundImageUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CTATextColor = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HomePages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SkillsCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    ImagePath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SkillsJson = table.Column<string>(type: "json", nullable: false),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SkillsCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Videos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    FilePath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ThumbnailPath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FileType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FileSize = table.Column<long>(type: "bigint", nullable: false),
                    Duration = table.Column<int>(type: "int", nullable: false),
                    UploadDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Videos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Education",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Institution = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Degree = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Field = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AboutId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Education", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Education_Abouts_AboutId",
                        column: x => x.AboutId,
                        principalTable: "Abouts",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "WorkExperience",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Company = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Position = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AchievementsJson = table.Column<string>(type: "json", nullable: false),
                    AboutId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkExperience", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkExperience_Abouts_AboutId",
                        column: x => x.AboutId,
                        principalTable: "Abouts",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Projects",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProjectUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GithubUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TechnologiesJson = table.Column<string>(type: "json", nullable: false),
                    CompletionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    HomePageId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Projects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Projects_HomePages_HomePageId",
                        column: x => x.HomePageId,
                        principalTable: "HomePages",
                        principalColumn: "Id");
                });

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
                name: "IX_Education_AboutId",
                table: "Education",
                column: "AboutId");

            migrationBuilder.CreateIndex(
                name: "IX_HomePages_DisplayOrder",
                table: "HomePages",
                column: "DisplayOrder");

            migrationBuilder.CreateIndex(
                name: "IX_HomePages_IsActive",
                table: "HomePages",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_HomePageId",
                table: "Projects",
                column: "HomePageId");

            migrationBuilder.CreateIndex(
                name: "IX_SkillsCategories_DisplayOrder",
                table: "SkillsCategories",
                column: "DisplayOrder");

            migrationBuilder.CreateIndex(
                name: "IX_SkillsCategories_IsActive",
                table: "SkillsCategories",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_WorkExperience_AboutId",
                table: "WorkExperience",
                column: "AboutId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Admins");

            migrationBuilder.DropTable(
                name: "Education");

            migrationBuilder.DropTable(
                name: "HeroTemplates");

            migrationBuilder.DropTable(
                name: "Projects");

            migrationBuilder.DropTable(
                name: "SkillsCategories");

            migrationBuilder.DropTable(
                name: "Videos");

            migrationBuilder.DropTable(
                name: "WorkExperience");

            migrationBuilder.DropTable(
                name: "HomePages");

            migrationBuilder.DropTable(
                name: "Abouts");
        }
    }
}
