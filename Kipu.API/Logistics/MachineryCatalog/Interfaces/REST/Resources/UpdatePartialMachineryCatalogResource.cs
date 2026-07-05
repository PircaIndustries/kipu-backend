using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;

namespace Kipu.API.Logistics.MachineryCatalog.Interfaces.REST.Resources;

[SwaggerSchema(Description = "Request payload to partially update a machinery catalog entry")]
public record UpdatePartialMachineryCatalogResource(
    [StringLength(200, MinimumLength = 1)]
    [SwaggerParameter(Description = "The machinery name (optional)")]
    string? Name,

    [StringLength(100)]
    [SwaggerParameter(Description = "The machinery brand (optional)")]
    string? Brand,

    [StringLength(100)]
    [SwaggerParameter(Description = "The machinery model (optional)")]
    string? Model,

    [StringLength(100)]
    [SwaggerParameter(Description = "The machinery serial number (optional)")]
    string? SerialNumber,

    [SwaggerParameter(Description = "The acquisition date (optional)")]
    string? AcquisitionDate
);
