using System;
using Microsoft.EntityFrameworkCore.Migrations;
using System.Security.Cryptography;
using System.Text;

#nullable disable

namespace Portfolio.Migrations
{
    /// <inheritdoc />
    public partial class AddAdminUserWithHashedPassword : Migration
    {
        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashedBytes);
            }
        }

        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var hashedPassword = HashPassword("Yareyou2");

            migrationBuilder.InsertData(
                table: "Admins",
                columns: new[] { "Username", "Email", "PasswordHash", "CreatedAt", "IsActive" },
                values: new object[] {
                    "admin",
                    "CynthiaSpinner@gmail.com",
                    hashedPassword,
                    DateTime.UtcNow,
                    true
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Admins",
                keyColumn: "Username",
                keyValue: "admin");
        }
    }
}
