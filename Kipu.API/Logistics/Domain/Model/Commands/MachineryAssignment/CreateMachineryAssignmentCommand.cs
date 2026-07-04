namespace Kipu.API.Logistics.Domain.Model.Commands;

public record CreateMachineryAssignmentCommand(
    string ProjectId,
    string MachineryId,
    string Name,
    string? MaintenanceHours,
    string? AssignmentDetail
);
