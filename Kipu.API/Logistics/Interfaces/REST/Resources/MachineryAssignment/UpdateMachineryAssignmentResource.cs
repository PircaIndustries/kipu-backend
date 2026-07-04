using Swashbuckle.AspNetCore.Annotations;

namespace Kipu.API.Logistics.Interfaces.REST.Resources.MachineryAssignment;

[SwaggerSchema(Description = "Request payload to partially update a machinery assignment")]
public record UpdateMachineryAssignmentResource(
    [SwaggerParameter(Description = "Machinery name/description")] string? Name,
    [SwaggerParameter(Description = "Current status (AVAILABLE, IN_USE, URGENT_MAINTENANCE)")] string? Status,
    [SwaggerParameter(Description = "Worker assigned (DNI - FullName)")] string? AssignedTo,
    [SwaggerParameter(Description = "Team worker ID assigned")] string? AssignedWorkerId,
    [SwaggerParameter(Description = "Maintenance hours accumulated")] string? MaintenanceHours,
    [SwaggerParameter(Description = "Assignment detail/purpose")] string? AssignmentDetail
);
