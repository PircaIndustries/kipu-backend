using Kipu.API.Projects.Domain.Model.Aggregates;
using Kipu.API.Projects.Domain.Repositories;
using Kipu.API.Shared.Infrastructure.Persistence.EFC.Configuration;
using Kipu.API.Shared.Infrastructure.Persistence.EFC.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Kipu.API.Projects.Infrastructure.Persistence.EFC.Repositories;

public class ProjectItemRepository(AppDbContext context) : BaseRepository<ProjectItem>(context), IProjectItemRepository
{
    public async Task<IEnumerable<ProjectItem>> FindByProjectIdAsync(int projectId)
    {
        return await Context.Set<ProjectItem>()
            .Where(pi => pi.ProjectId == projectId)
            .ToListAsync();
    }
}
