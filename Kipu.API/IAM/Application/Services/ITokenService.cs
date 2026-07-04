using Kipu.API.IAM.Domain.Model.Aggregates;

namespace Kipu.API.IAM.Application.Services;

public interface ITokenService
{
    string GenerateToken(User user, bool rememberMe = false);
}
