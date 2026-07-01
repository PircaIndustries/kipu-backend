using Kipu.API.Logistics.Domain.Model.Aggregates;
using Kipu.API.Shared.Domain.Repositories;

namespace Kipu.API.Logistics.Domain.Repositories;

public interface IMachineryAssignmentRepository : IBaseRepository<MachineryAssignment>
{
    Task<IEnumerable<MachineryAssignment>> FindByProjectIdAsync(string projectId, CancellationToken cancellationToken = default);
}
