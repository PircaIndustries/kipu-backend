using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;

namespace Kipu.API.Logistics.Interfaces.REST.Resources;

/// <summary>
/// Request payload to create a new material inventory
/// </summary>
/// <param name="ProjectId">The project identifier</param>
/// <param name="MaterialId">The material catalog identifier</param>
/// <param name="CurrentStock">Current available stock quantity</param>
/// <param name="MinimumStock">Minimum stock level that triggers alert</param>
/// <param name="Location">Physical storage location</param>
[SwaggerSchema(Description = "Request payload to create a new material inventory")]
public record CreateMaterialInventoryResource(
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