using Kipu.API.Logistics.MachineryCatalog.Domain.Model.Commands;
using Kipu.API.Shared.Domain.Model;

namespace Kipu.API.Logistics.MachineryCatalog.Domain.Model.Aggregates;

public class MachineryCatalog : IAuditableEntity
{
    protected MachineryCatalog()
    {
        Id = Guid.NewGuid().ToString();
        Name = string.Empty;
        Brand = string.Empty;
        Model = string.Empty;
        SerialNumber = string.Empty;
    }

    public MachineryCatalog(CreateMachineryCatalogCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);
        Id = Guid.NewGuid().ToString();
        Name = command.Name;
        Brand = command.Brand;
        Model = command.Model;
        SerialNumber = command.SerialNumber;
        AcquisitionDate = command.AcquisitionDate;
        CreatedAt = DateTimeOffset.UtcNow;
    }

    public string Id { get; private set; }
    public string Name { get; private set; }
    public string Brand { get; private set; }
    public string Model { get; private set; }
    public string SerialNumber { get; private set; }
    public string? AcquisitionDate { get; private set; }
    public DateTimeOffset? CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
}
