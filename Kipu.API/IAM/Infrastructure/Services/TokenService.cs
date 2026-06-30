using Kipu.API.IAM.Application.Services;
using Kipu.API.IAM.Domain.Model.Aggregates;

namespace Kipu.API.IAM.Infrastructure.Services;

public class TokenService : ITokenService
{
    public string GenerateToken(User user, bool rememberMe = false)
    {
        var expiration = rememberMe ? "30d" : "1h";
        return $"dummy-jwt-token-for-user-{user.Id}-{user.Email}-{user.Role}-exp-{expiration}";
    }
}
