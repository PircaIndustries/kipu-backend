using Swashbuckle.AspNetCore.Annotations;
using Kipu.API.Logistics.Domain.Model.ValueObjects;

namespace Kipu.API.Logistics.Interfaces.REST.Resources;

/// <summary>
/// Material catalog resource returned by the API
/// </summary>
/// <param name="Id">The server-generated ID of the material catalog</param>
/// <param name="Name">The material name</param>
/// <param name="CategoryId">The category identifier</param>
/// <param name="MeasureUnit">The unit of measurement</param>
[SwaggerSchema(Description = "A material catalog resource")]
public record MaterialCatalogResource(
    [SwaggerParameter(Description = "The server-generated ID of the material catalog")] 
    int Id,
    
    [SwaggerParameter(Description = "The material name")] 
    string Name,
    
    [SwaggerParameter(Description = "The category identifier")] 
    int CategoryId,
    
    [SwaggerParameter(Description = "The unit of measurement")] 
    MeasureUnit MeasureUnit);