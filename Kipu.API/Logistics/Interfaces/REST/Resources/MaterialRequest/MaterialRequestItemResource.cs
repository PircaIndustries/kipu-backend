using Swashbuckle.AspNetCore.Annotations;

namespace Kipu.API.Logistics.Interfaces.REST.Resources.MaterialRequest;

[SwaggerSchema(Description = "A material request item resource")]
public record MaterialRequestItemResource(
    [SwaggerParameter(Description = "The server-generated ID of the item")]
    int Id,

    [SwaggerParameter(Description = "The material catalog ID")]
    int MaterialCatalogId,

    [SwaggerParameter(Description = "The supplier ID")]
    int SupplierId,

    [SwaggerParameter(Description = "The requested quantity")]
    decimal Quantity,

    [SwaggerParameter(Description = "The unit price from the supplier")]
    decimal UnitPrice);
