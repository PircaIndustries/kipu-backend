using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;

namespace Kipu.API.Logistics.Interfaces.REST.Resources.MaterialRequest;

[SwaggerSchema(Description = "An item within a material request")]
public record CreateMaterialRequestItemResource(
    [Required]
    [SwaggerParameter(Description = "The material catalog ID")]
    int MaterialCatalogId,

    [Required]
    [SwaggerParameter(Description = "The supplier ID")]
    int SupplierId,

    [Required]
    [Range(0.01, double.MaxValue)]
    [SwaggerParameter(Description = "The requested quantity")]
    decimal Quantity,

    [Required]
    [Range(0.01, double.MaxValue)]
    [SwaggerParameter(Description = "The unit price from the supplier")]
    decimal UnitPrice);
