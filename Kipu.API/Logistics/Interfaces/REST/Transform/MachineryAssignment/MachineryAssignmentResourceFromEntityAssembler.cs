using Kipu.API.Logistics.Domain.Model.Aggregates;
using Kipu.API.Logistics.Interfaces.REST.Resources.MachineryAssignment;

namespace Kipu.API.Logistics.Interfaces.REST.Transform.MachineryAssignment;

public static class MachineryAssignmentResourceFromEntityAssembler
{
    public static MachineryAssignmentResource ToResourceFromEntity(Domain.Model.Aggregates.MachineryAssignment entity) =>
        new(
            entity.Id,
            entity.ProjectId,
            entity.MachineryId,
            entity.Name,
            entity.Status,
            entity.AssignedTo,
            entity.RegistrationDate.ToString("o"),
            entity.MaintenanceHours,
            entity.AssignmentDetail
        );
}
