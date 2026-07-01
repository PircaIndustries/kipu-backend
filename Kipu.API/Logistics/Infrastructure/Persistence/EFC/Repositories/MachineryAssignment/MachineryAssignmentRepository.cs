using Kipu.API.Logistics.Domain.Model.Aggregates;
using Kipu.API.Logistics.Domain.Repositories;
using Kipu.API.Shared.Infrastructure.Persistence.EFC.Repositories;

namespace Kipu.API.Logistics.Infrastructure.Persistence.EFC.Repositories;

public class MachineryAssignmentRepository(AppDbContext context)
    : BaseRepository<MachineryAssignment>(context), IMachineryAssignmentRepository
{
    public async Task<IEnumerable<MachineryAssignment>> FindByProjectIdAsync(string projectId, CancellationToken cancellationToken = default)
    {
        return await Context.Set<MachineryAssignment>()
            .Where(a => a.ProjectId == projectId)
            .ToListAsync(cancellationToken);
    }
}
