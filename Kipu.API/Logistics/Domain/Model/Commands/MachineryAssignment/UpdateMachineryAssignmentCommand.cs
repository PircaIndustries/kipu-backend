namespace Kipu.API.Logistics.Domain.Model.Commands;

public record UpdateMachineryAssignmentCommand(
    string Id,
    string? Name,
    string? Status,
    string? AssignedTo,
    string? AssignedWorkerId,
    string? MaintenanceHours,
    string? AssignmentDetail
);
