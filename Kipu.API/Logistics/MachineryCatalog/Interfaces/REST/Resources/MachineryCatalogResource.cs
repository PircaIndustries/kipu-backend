using Swashbuckle.AspNetCore.Annotations;

namespace Kipu.API.Logistics.MachineryCatalog.Interfaces.REST.Resources;

[SwaggerSchema(Description = "A machinery catalog resource")]
public record MachineryCatalogResource(
    string Id,
    string Name,
    string Brand,
    string Model,
    string SerialNumber,
    string? AcquisitionDate
);
