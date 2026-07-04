using Kipu.API.Logistics.Domain.Model.Aggregates;
using Kipu.API.Logistics.Domain.Model.Queries;

namespace Kipu.API.Logistics.Application.Services;

public interface IMachineryAssignmentQueryService
{
    Task<MachineryAssignment?> Handle(GetMachineryAssignmentByIdQuery query, CancellationToken cancellationToken = default);
    Task<IEnumerable<MachineryAssignment>> Handle(GetMachineryAssignmentsByProjectIdQuery query, CancellationToken cancellationToken = default);
}
