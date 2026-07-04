using Kipu.API.Logistics.MachineryCatalog.Domain.Model.Aggregates;
using Kipu.API.Logistics.MachineryCatalog.Interfaces.REST.Resources;

namespace Kipu.API.Logistics.MachineryCatalog.Interfaces.REST.Transform;

public static class MachineryCatalogResourceFromEntityAssembler
{
    public static MachineryCatalogResource ToResourceFromEntity(Domain.Model.Aggregates.MachineryCatalog entity) =>
        new(entity.Id, entity.Name, entity.Brand, entity.Model, entity.SerialNumber, entity.AcquisitionDate);
}
