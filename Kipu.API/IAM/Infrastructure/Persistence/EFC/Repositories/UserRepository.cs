using Kipu.API.IAM.Domain.Model.Aggregates;
using Kipu.API.IAM.Domain.Repositories;
using Kipu.API.Shared.Infrastructure.Persistence.EFC.Configuration;
using Kipu.API.Shared.Infrastructure.Persistence.EFC.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Kipu.API.IAM.Infrastructure.Persistence.EFC.Repositories;

public class UserRepository(AppDbContext context) : BaseRepository<User>(context), IUserRepository
{
    public async Task<User?> FindByEmailAsync(string email)
    {
        return await Context.Set<User>().FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<bool> ExistsByEmailAsync(string email)
    {
        return await Context.Set<User>().AnyAsync(u => u.Email == email);
    }
}
