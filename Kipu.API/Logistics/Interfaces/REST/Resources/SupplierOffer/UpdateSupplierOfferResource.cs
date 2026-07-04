using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;

namespace Kipu.API.Logistics.Interfaces.REST.Resources.SupplierOffer;

[SwaggerSchema(Description = "Request payload to update a supplier offer")]
public record UpdateSupplierOfferResource(
    [Required] [SwaggerParameter(Description = "The supplier ID")] int SupplierId,
    [Required] [SwaggerParameter(Description = "The material catalog ID")] int MaterialId,
    [Required] [Range(0.01, double.MaxValue)] [SwaggerParameter(Description = "Unit price")] decimal UnitPrice);
