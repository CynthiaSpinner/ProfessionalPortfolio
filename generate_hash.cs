using System;
using System.Security.Cryptography;
using System.Text;

class Program
{
    static void Main()
    {
        string password = "Yareyou2";
        string hash = HashPassword(password);
        Console.WriteLine($"Password: {password}");
        Console.WriteLine($"SHA256 Hash: {hash}");
        
        Console.WriteLine("\nSQL Query:");
        Console.WriteLine($"UPDATE Admins SET Username = 'admin', Email = 'CynthiaSpinner@gmail.com', PasswordHash = '{hash}', UpdatedAt = GETDATE() WHERE Id = 1;");
    }
    
    static string HashPassword(string password)
    {
        using (var sha256 = SHA256.Create())
        {
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hashedBytes);
        }
    }
} 