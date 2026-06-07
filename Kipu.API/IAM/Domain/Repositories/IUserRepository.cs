using Kipu.API.IAM.Domain.Model.Aggregates;
using Kipu.API.Shared.Domain.Repositories;

namespace Kipu.API.IAM.Domain.Repositories;

public interface IUserRepository : IBaseRepository<User>
{
    Task<User?> FindByEmailAsync(string email);
    Task<bool> ExistsByEmailAsync(string email);
}