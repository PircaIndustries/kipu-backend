using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Kipu.API.IAM.Application.Services;
using Kipu.API.IAM.Domain.Model.Aggregates;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Kipu.API.IAM.Infrastructure.Services;

public class TokenService(IConfiguration configuration) : ITokenService
{
    public string GenerateToken(User user, bool rememberMe = false)
    {
        var secret = configuration["AppSettings:Secret"] ?? throw new InvalidOperationException("Secret not found in appsettings.");
        var key = Encoding.ASCII.GetBytes(secret);
        
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(
            [
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role)
            ]),
            Expires = rememberMe ? DateTime.UtcNow.AddDays(30) : DateTime.UtcNow.AddHours(1),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        
        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}
