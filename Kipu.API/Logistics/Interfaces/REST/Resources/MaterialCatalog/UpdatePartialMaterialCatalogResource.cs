using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;
using Kipu.API.Logistics.Domain.Model.ValueObjects;

namespace Kipu.API.Logistics.Interfaces.REST.Resources;

[SwaggerSchema(Description = "Request payload to partially update a material catalog entry")]
public record UpdatePartialMaterialCatalogResource(
    [StringLength(100, MinimumLength = 1)]
    [SwaggerParameter(Description = "The material name (optional)")]
    string? Name,

    [SwaggerParameter(Description = "The category identifier (optional)")]
    int? CategoryId,

    [SwaggerParameter(Description = "The unit of measurement (optional)")]
    MeasureUnit? MeasureUnit
);
