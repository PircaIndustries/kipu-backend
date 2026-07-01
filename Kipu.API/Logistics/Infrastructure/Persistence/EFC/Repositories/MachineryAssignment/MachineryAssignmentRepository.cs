using Kipu.API.Logistics.Domain.Model.Aggregates;
using Kipu.API.Logistics.Domain.Repositories;
using Kipu.API.Shared.Infrastructure.Persistence.EFC.Configuration;
using Kipu.API.Shared.Infrastructure.Persistence.EFC.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Kipu.API.Logistics.Infrastructure.Persistence.EFC.Repositories;

public class MachineryAssignmentRepository(AppDbContext context)
    : BaseAggregateRepository<MachineryAssignment, string>(context), IMachineryAssignmentRepository
{
    public async Task<IEnumerable<MachineryAssignment>> FindByProjectIdAsync(string projectId, CancellationToken cancellationToken = default)
    {
        return await Context.Set<MachineryAssignment>()
            .Where(a => a.ProjectId == projectId)
            .ToListAsync(cancellationToken);
    }
}
