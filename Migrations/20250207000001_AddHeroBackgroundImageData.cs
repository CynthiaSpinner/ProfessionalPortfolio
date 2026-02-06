using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Portfolio.Migrations
{
    /// <inheritdoc />
    public partial class AddHeroBackgroundImageData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "HeaderBackgroundImageData",
                table: "HomePages",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HeaderBackgroundImageContentType",
                table: "HomePages",
                maxLength: 100,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HeaderBackgroundImageData",
                table: "HomePages");

            migrationBuilder.DropColumn(
                name: "HeaderBackgroundImageContentType",
                table: "HomePages");
        }
    }
}
