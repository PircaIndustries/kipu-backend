using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;
using Kipu.API.Logistics.Domain.Model.ValueObjects;

namespace Kipu.API.Logistics.Interfaces.REST.Resources;

[SwaggerSchema(Description = "Request payload to update a material catalog entry")]
public record UpdateMaterialCatalogResource(
    [Required]
    [SwaggerParameter(Description = "The material name")]
    string Name,

    [Required]
    [SwaggerParameter(Description = "The category identifier")]
    int CategoryId,

    [Required]
    [SwaggerParameter(Description = "The unit of measurement")]
    MeasureUnit MeasureUnit);
