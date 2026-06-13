using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;

namespace Kipu.API.Logistics.Interfaces.REST.Resources.MaterialRequest;

[SwaggerSchema(Description = "Request payload to partially update an existing material request")]
public record UpdatePartialMaterialRequestResource(
    [SwaggerParameter(Description = "The deadline for the request (optional)")]
    DateTime? Deadline,

    [SwaggerParameter(Description = "The priority of the request (Low, Medium, High, Critical) (optional)")]
    string? RequestPriority,

    [StringLength(500)]
    [SwaggerParameter(Description = "The delivery location (optional)")]
    string? DeliveryLocation,

    [SwaggerParameter(Description = "The budget line ID (optional)")]
    int? BudgetLineId,

    [StringLength(1000)]
    [SwaggerParameter(Description = "The purpose of the request (optional)")]
    string? Purpose,

    [StringLength(2000)]
    [SwaggerParameter(Description = "Additional notes (optional)")]
    string? AdditionalNotes,

    [SwaggerParameter(Description = "The items to add or update in the request (optional). Items with an existing ID are updated, items without ID are added.")]
    List<UpdatePartialMaterialRequestItemResource>? Items);
