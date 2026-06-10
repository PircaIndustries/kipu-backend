using Swashbuckle.AspNetCore.Annotations;

namespace Kipu.API.Logistics.Interfaces.REST.Resources;

/// <summary>
/// Material inventory resource returned by the API
/// </summary>
/// <param name="Id">The server-generated ID of the material inventory</param>
/// <param name="ProjectId">The project identifier</param>
/// <param name="MaterialId">The material catalog identifier</param>
/// <param name="CurrentStock">Current available stock quantity</param>
/// <param name="MinimumStock">Minimum stock level that triggers alert</param>
/// <param name="Location">Physical storage location</param>
/// <param name="CreatedAt">Creation timestamp</param>
/// <param name="UpdatedAt">Last update timestamp</param>
[SwaggerSchema(Description = "A material inventory resource")]
public record MaterialInventoryResource(
    [SwaggerParameter(Description = "The server-generated ID of the material inventory")] 
    int Id,
    
    [SwaggerParameter(Description = "The project identifier")] 
    int ProjectId,
    
    [SwaggerParameter(Description = "The material catalog identifier")] 
    int MaterialId,
    
    [SwaggerParameter(Description = "Current available stock quantity")] 
    int CurrentStock,
    
    [SwaggerParameter(Description = "Minimum stock level that triggers alert")] 
    int MinimumStock,
    
    [SwaggerParameter(Description = "Physical storage location")] 
    string Location);