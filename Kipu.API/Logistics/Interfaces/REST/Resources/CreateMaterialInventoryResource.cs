using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;
namespace Kipu.API.Logistics.Interfaces.REST.Resources;

[SwaggerSchema(Description = "Request payload to create a MaterialInventory")]
public record CreateMaterialInventoryResource(
    [Required]
    [SwaggerParameter(Description = "The ProjectId")] int ProjectId,
    [Required]
    [SwaggerParameter(Description = "The MaterialId")] int MaterialId,
    [Required]
    [SwaggerParameter(Description = "The CurrentStock")] int CurrentStock,
    [Required]
    [SwaggerParameter(Description = "The MinimumStock")] int MinimumStock,
    [Required]
    [SwaggerParameter(Description = "The Location")] string Location);