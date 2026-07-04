using Kipu.API.Logistics.Application.Errors;
using Kipu.API.Logistics.Domain.Model.Aggregates;
using Kipu.API.Logistics.Domain.Model.Commands;
using Kipu.API.Shared.Application.Patterns;

namespace Kipu.API.Logistics.Application.Services;

public interface IMachineryAssignmentCommandService
{
    Task<Result<MachineryAssignment, CreateMachineryAssignmentError>> Handle(CreateMachineryAssignmentCommand command, CancellationToken cancellationToken = default);
    Task<Result<MachineryAssignment, UpdateMachineryAssignmentError>> Handle(UpdateMachineryAssignmentCommand command, CancellationToken cancellationToken = default);
    Task<Result<MachineryAssignment, UpdateMachineryAssignmentError>> HandleDelete(string id, CancellationToken cancellationToken = default);
}
