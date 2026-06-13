using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;

namespace Kipu.API.Logistics.Interfaces.REST.Resources.MaterialRequest;

[SwaggerSchema(Description = "Request payload to create a new material request")]
public record CreateMaterialRequestResource(
    [Required]
    [SwaggerParameter(Description = "The deadline for the request")]
    DateTime Deadline,

    [Required]
    [SwaggerParameter(Description = "The priority of the request (Low, Medium, High, Critical)")]
    string RequestPriority,

    [Required]
    [StringLength(500)]
    [SwaggerParameter(Description = "The delivery location")]
    string DeliveryLocation,

    [Required]
    [SwaggerParameter(Description = "The budget line ID")]
    int BudgetLineId,

    [Required]
    [StringLength(1000)]
    [SwaggerParameter(Description = "The purpose of the request")]
    string Purpose,

    [StringLength(2000)]
    [SwaggerParameter(Description = "Additional notes")]
    string? AdditionalNotes,

    [Required]
    [SwaggerParameter(Description = "The ID of the user who requested")]
    int RequestedBy,

    [Required]
    [MinLength(1)]
    [SwaggerParameter(Description = "The items to include in the request")]
    List<CreateMaterialRequestItemResource> Items);
