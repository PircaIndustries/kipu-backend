namespace Kipu.API.Logistics.MachineryCatalog.Domain.Model.Commands;

public record UpdateMachineryCatalogPartialCommand(
    string Id,
    string? Name = null,
    string? Brand = null,
    string? Model = null,
    string? SerialNumber = null,
    string? AcquisitionDate = null
);
