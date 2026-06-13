using Swashbuckle.AspNetCore.Annotations;

namespace Kipu.API.Logistics.Interfaces.REST.Resources.MaterialRequest;

[SwaggerSchema(Description = "A material request resource")]
public record MaterialRequestResource(
    [SwaggerParameter(Description = "The server-generated ID of the request")]
    int Id,

    [SwaggerParameter(Description = "The deadline for the request")]
    DateTime Deadline,

    [SwaggerParameter(Description = "The status of the request (Pending, Accepted, Refused)")]
    string RequestStatus,

    [SwaggerParameter(Description = "The priority of the request")]
    string RequestPriority,

    [SwaggerParameter(Description = "The delivery location")]
    string DeliveryLocation,

    [SwaggerParameter(Description = "The budget line ID")]
    int BudgetLineId,

    [SwaggerParameter(Description = "The purpose of the request")]
    string Purpose,

    [SwaggerParameter(Description = "Additional notes")]
    string AdditionalNotes,

    [SwaggerParameter(Description = "The ID of the user who requested")]
    int RequestedBy,

    [SwaggerParameter(Description = "The items in the request")]
    List<MaterialRequestItemResource> Items);
