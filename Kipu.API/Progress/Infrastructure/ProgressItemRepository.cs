using Kipu.API.Progress.Domain.Model.Aggregates;
using Kipu.API.Progress.Domain.Repositories;
using Kipu.API.Shared.Infrastructure.Persistence.EFC.Configuration;
using Kipu.API.Shared.Infrastructure.Persistence.EFC.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Kipu.API.Progress.Infrastructure;

public class ProgressItemRepository(AppDbContext context) : BaseRepository<ProgressItem>(context), IProgressItemRepository
{
    public async Task<IEnumerable<ProgressItem>> FindByProjectIdAsync(int projectId)
    {
        return await Context.Set<ProgressItem>().Where(p => p.ProjectId == projectId).ToListAsync();
    }
}