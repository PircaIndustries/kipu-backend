using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;
using Kipu.API.Logistics.Domain.Model.ValueObjects;

namespace Kipu.API.Logistics.Interfaces.REST.Resources;

/// <summary>
/// Request payload to create a new material catalog entry
/// </summary>
/// <param name="Name">The material name</param>
/// <param name="CategoryId">The category identifier</param>
/// <param name="MeasureUnit">The unit of measurement</param>
[SwaggerSchema(Description = "Request payload to create a new material catalog entry")]
public record CreateMaterialCatalogResource(
    [Required]
    [SwaggerParameter(Description = "The material name")] 
    string Name,
    
    [Required]
    [SwaggerParameter(Description = "The category identifier")] 
    int CategoryId,
    
    [Required]
    [SwaggerParameter(Description = "The unit of measurement")] 
    MeasureUnit MeasureUnit);