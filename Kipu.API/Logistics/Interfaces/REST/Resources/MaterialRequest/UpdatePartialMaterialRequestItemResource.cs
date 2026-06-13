using Swashbuckle.AspNetCore.Annotations;

namespace Kipu.API.Logistics.Interfaces.REST.Resources.MaterialRequest;

[SwaggerSchema(Description = "An item to partially update within a material request")]
public record UpdatePartialMaterialRequestItemResource(
    [SwaggerParameter(Description = "The item ID (omit or 0 to add a new item)")]
    int? Id,

    [SwaggerParameter(Description = "The material catalog ID (optional)")]
    int? MaterialCatalogId,

    [SwaggerParameter(Description = "The supplier ID (optional)")]
    int? SupplierId,

    [SwaggerParameter(Description = "The requested quantity (optional)")]
    decimal? Quantity,

    [SwaggerParameter(Description = "The unit price from the supplier (optional)")]
    decimal? UnitPrice);
