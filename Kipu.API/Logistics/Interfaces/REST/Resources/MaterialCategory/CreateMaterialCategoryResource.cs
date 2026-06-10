using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;

namespace Kipu.API.Logistics.Interfaces.REST.Resources.MaterialCategory;

/// <summary>
///     Represents the data required to create a material category.
/// </summary>
/// <param name="Name">The category name (required)</param>
/// <param name="Description">The category description (optional)</param>
/// <param name="IsActive">Whether the category is active (default: true)</param>
[SwaggerSchema(Description = "Request payload to create a material category")]
public record CreateMaterialCategoryResource(
    [Required]
    [StringLength(100, MinimumLength = 3)]
    [SwaggerParameter(Description = "The category name")]
    string Name,
    
    [SwaggerParameter(Description = "The category description")]
    string? Description,
    
    [SwaggerParameter(Description = "Whether the category is active")]
    bool IsActive = true);