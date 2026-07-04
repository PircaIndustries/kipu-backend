using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;

namespace Kipu.API.Logistics.MachineryCatalog.Interfaces.REST.Resources;

[SwaggerSchema(Description = "Request payload to create a new machinery catalog entry")]
public record CreateMachineryCatalogResource(
    [Required] string Name,
    [Required] string Brand,
    [Required] string Model,
    [Required] string SerialNumber,
    string? AcquisitionDate
);
