using Swashbuckle.AspNetCore.Annotations;

namespace Kipu.API.Logistics.Interfaces.REST.Resources.SupplierOffer;

[SwaggerSchema(Description = "A supplier offer resource")]
public record SupplierOfferResource(
    [SwaggerParameter(Description = "The server-generated ID")] int Id,
    [SwaggerParameter(Description = "The supplier ID")] int SupplierId,
    [SwaggerParameter(Description = "The material catalog ID")] int MaterialId,
    [SwaggerParameter(Description = "Unit price")] decimal UnitPrice);
