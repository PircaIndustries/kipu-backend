using Kipu.API.Logistics.Application.Services;
using Kipu.API.Logistics.Domain.Model.Aggregates;
using Kipu.API.Logistics.Domain.Model.Queries;
using Kipu.API.Logistics.Domain.Repositories;

namespace Kipu.API.Logistics.Application.Internal.QueryServices;

public class MachineryAssignmentQueryService(
    IMachineryAssignmentRepository machineryAssignmentRepository,
    ILogger<MachineryAssignmentQueryService> logger)
    : IMachineryAssignmentQueryService
{
    public async Task<MachineryAssignment?> Handle(
        GetMachineryAssignmentByIdQuery query, CancellationToken cancellationToken = default)
    {
        return await machineryAssignmentRepository.FindByIdAsync(query.Id, cancellationToken);
    }

    public async Task<IEnumerable<MachineryAssignment>> Handle(
        GetMachineryAssignmentsByProjectIdQuery query, CancellationToken cancellationToken = default)
    {
        return await machineryAssignmentRepository.FindByProjectIdAsync(query.ProjectId, cancellationToken);
    }
}
