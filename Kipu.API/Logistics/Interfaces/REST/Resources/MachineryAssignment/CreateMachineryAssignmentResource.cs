using Swashbuckle.AspNetCore.Annotations;

namespace Kipu.API.Logistics.Interfaces.REST.Resources.MachineryAssignment;

[SwaggerSchema(Description = "Request payload to create a new machinery assignment")]
public record CreateMachineryAssignmentResource(
    [Required][SwaggerParameter(Description = "Project identifier")] string ProjectId,
    [Required][SwaggerParameter(Description = "Machinery catalog identifier")] string MachineryId,
    [Required][SwaggerParameter(Description = "Machinery name/description")] string Name,
    [SwaggerParameter(Description = "Maintenance hours")] string? MaintenanceHours,
    [SwaggerParameter(Description = "Assignment detail/purpose")] string? AssignmentDetail
);
