using Kipu.API.Logistics.Domain.Model.Commands;
using Kipu.API.Logistics.Interfaces.REST.Resources.MachineryAssignment;

namespace Kipu.API.Logistics.Interfaces.REST.Transform.MachineryAssignment;

public static class CreateMachineryAssignmentCommandFromResourceAssembler
{
    public static CreateMachineryAssignmentCommand ToCommandFromResource(CreateMachineryAssignmentResource resource) =>
        new(
            resource.ProjectId,
            resource.MachineryId,
            resource.Name,
            resource.MaintenanceHours,
            resource.AssignmentDetail
        );
}
