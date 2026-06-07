using Kipu.API.IAM.Application.Services;
using System.Security.Cryptography;
using System.Text;

namespace Kipu.API.IAM.Infrastructure.Services;

public class HashingService : IHashingService
{
    public string HashPassword(string password)
    {
        using var sha256 = SHA256.Create();
        var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
        return Convert.ToBase64String(bytes);
    }

    public bool VerifyPassword(string password, string passwordHash)
    {
        return HashPassword(password) == passwordHash;
    }
}
