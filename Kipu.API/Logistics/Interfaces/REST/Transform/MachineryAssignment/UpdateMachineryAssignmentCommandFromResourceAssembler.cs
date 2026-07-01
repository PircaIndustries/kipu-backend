using Kipu.API.Logistics.Domain.Model.Commands;
using Kipu.API.Logistics.Interfaces.REST.Resources.MachineryAssignment;

namespace Kipu.API.Logistics.Interfaces.REST.Transform.MachineryAssignment;

public static class UpdateMachineryAssignmentCommandFromResourceAssembler
{
    public static UpdateMachineryAssignmentCommand ToCommandFromResource(string id, UpdateMachineryAssignmentResource resource) =>
        new(
            id,
            resource.Name,
            resource.Status,
            resource.AssignedTo,
            resource.AssignedWorkerId,
            resource.MaintenanceHours,
            resource.AssignmentDetail
        );
}
