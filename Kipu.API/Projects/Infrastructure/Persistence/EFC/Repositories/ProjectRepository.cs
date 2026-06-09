using Kipu.API.Projects.Domain.Model.Aggregates;
using Kipu.API.Projects.Domain.Repositories;
using Kipu.API.Shared.Infrastructure.Persistence.EFC.Configuration;
using Kipu.API.Shared.Infrastructure.Persistence.EFC.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Kipu.API.Projects.Infrastructure.Persistence.EFC.Repositories;

public class ProjectRepository(AppDbContext context) : BaseRepository<Project>(context), IProjectRepository
{
    public async Task<Project?> FindByNameAsync(string name)
    {
        return await Context.Set<Project>().FirstOrDefaultAsync(p => p.Name == name);
    }

    public async Task<bool> ExistsByNameAsync(string name)
    {
        return await Context.Set<Project>().AnyAsync(p => p.Name == name);
    }
}
