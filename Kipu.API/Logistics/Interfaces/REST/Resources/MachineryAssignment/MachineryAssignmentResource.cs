using Swashbuckle.AspNetCore.Annotations;

namespace Kipu.API.Logistics.Interfaces.REST.Resources.MachineryAssignment;

[SwaggerSchema(Description = "A machinery assignment resource")]
public record MachineryAssignmentResource(
    [SwaggerParameter(Description = "Unique identifier")] string Id,
    [SwaggerParameter(Description = "Project identifier")] string ProjectId,
    [SwaggerParameter(Description = "Machinery catalog identifier")] string MachineryId,
    [SwaggerParameter(Description = "Machinery name/description")] string Name,
    [SwaggerParameter(Description = "Current status")] string Status,
    [SwaggerParameter(Description = "Worker assigned")] string? AssignedTo,
    [SwaggerParameter(Description = "Registration date")] string RegistrationDate,
    [SwaggerParameter(Description = "Maintenance hours")] string MaintenanceHours,
    [SwaggerParameter(Description = "Assignment detail/purpose")] string AssignmentDetail
);
