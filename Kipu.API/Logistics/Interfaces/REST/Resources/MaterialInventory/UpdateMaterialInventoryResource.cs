using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;

namespace Kipu.API.Logistics.Interfaces.REST.Resources;

[SwaggerSchema(Description = "Request payload to update a material inventory")]
public record UpdateMaterialInventoryResource(
    [Required]
    [SwaggerParameter(Description = "The project identifier")]
    int ProjectId,

    [Required]
    [SwaggerParameter(Description = "The material catalog identifier")]
    int MaterialId,

    [Required]
    [SwaggerParameter(Description = "Current available stock quantity")]
    int CurrentStock,

    [Required]
    [SwaggerParameter(Description = "Minimum stock level that triggers alert")]
    int MinimumStock,

    [Required]
    [SwaggerParameter(Description = "Physical storage location")]
    string Location);
