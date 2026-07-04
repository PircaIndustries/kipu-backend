namespace Kipu.API.Logistics.MachineryCatalog.Domain.Model.Commands;

public record CreateMachineryCatalogCommand(
    string Name,
    string Brand,
    string Model,
    string SerialNumber,
    string? AcquisitionDate
);
