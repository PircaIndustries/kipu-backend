using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;

namespace Kipu.API.Logistics.Interfaces.REST.Resources.MaterialCategory;

[SwaggerSchema(Description = "Request payload to update a material category")]
public record UpdateMaterialCategoryResource(
    [Required]
    [StringLength(100, MinimumLength = 3)]
    [SwaggerParameter(Description = "The category name")]
    string Name,

    [SwaggerParameter(Description = "The category description")]
    string? Description,

    [SwaggerParameter(Description = "Whether the category is active")]
    bool IsActive);
