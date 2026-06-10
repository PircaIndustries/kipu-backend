using Swashbuckle.AspNetCore.Annotations;

namespace Kipu.API.Logistics.Interfaces.REST.Resources.MaterialCategory;

/// <summary>
///     Represents the data provided by the server about a material category.
/// </summary>
/// <param name="Id">The server-generated ID of the material category</param>
/// <param name="Name">The category name</param>
/// <param name="Description">The category description</param>
/// <param name="IsActive">Whether the category is active</param>
[SwaggerSchema(Description = "A material category resource")]
public record MaterialCategoryResource(
    [SwaggerParameter(Description = "The server-generated ID of the material category")]
    int Id,
    
    [SwaggerParameter(Description = "The category name")]
    string Name,
    
    [SwaggerParameter(Description = "The category description")]
    string Description,
    
    [SwaggerParameter(Description = "Whether the category is active")]
    bool IsActive);