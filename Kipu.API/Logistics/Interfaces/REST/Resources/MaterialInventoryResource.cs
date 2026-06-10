namespace Kipu.API.Logistics.Interfaces.REST.Resources;
using Swashbuckle.AspNetCore.Annotations;

[SwaggerSchema(Description = "A MaterialInventory resource")]
public record MaterialInventoryResource(
    [Required]
    [SwaggerParameter(Description = "The Id")] int Id,
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