using Kipu.API.IAM.Domain.Model.Aggregates;

namespace Kipu.API.IAM.Domain.Services;

public interface IUserService
{
    Task<User> RegisterAsync(User user, string rawPassword);
    Task<User?> AuthenticateAsync(string email, string password);
}