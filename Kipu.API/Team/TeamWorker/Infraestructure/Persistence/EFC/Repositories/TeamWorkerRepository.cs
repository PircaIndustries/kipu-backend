using Kipu.API.Shared.Infrastructure.Persistence.EFC.Configuration;
using Kipu.API.Shared.Infrastructure.Persistence.EFC.Repositories;
using Kipu.API.Team.TeamWorker.Domain.Model.ValueObjects;
using Kipu.API.Team.TeamWorker.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Kipu.API.Team.TeamWorker.Infraestructure.Persistence.EFC.Repositories;

public class TeamWorkerRepository : BaseAggregateRepository<Domain.Model.Aggregates.TeamWorker, WorkerId>, ITeamWorkerRepository
{
    public TeamWorkerRepository(AppDbContext context) : base(context)
    {
    }

    public new async Task<Domain.Model.Aggregates.TeamWorker?> FindById(WorkerId id)
    {
        return await Context.Set<Domain.Model.Aggregates.TeamWorker>()
            .Include(w => w.Machineries) 
            .FirstOrDefaultAsync(w => w.Id == id);
    }

    public async Task<IEnumerable<Domain.Model.Aggregates.TeamWorker>> FindByProjectIdAsync(string projectId, string? globalSearch)
    {
        var query = Context.Set<Domain.Model.Aggregates.TeamWorker>()
            .Include(w => w.Machineries)
            .Where(w => w.ProjectId == projectId);

        if (!string.IsNullOrWhiteSpace(globalSearch))
        {
            query = query.Where(w => 
                w.Dni.Contains(globalSearch) || 
                w.FullName.Contains(globalSearch) || 
                w.Role.Contains(globalSearch)
            );
        }

        return await query.ToListAsync();
    }
}