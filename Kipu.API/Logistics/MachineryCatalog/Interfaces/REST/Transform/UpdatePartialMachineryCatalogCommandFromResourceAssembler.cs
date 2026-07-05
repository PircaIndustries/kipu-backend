using Kipu.API.Logistics.MachineryCatalog.Domain.Model.Commands;
using Kipu.API.Logistics.MachineryCatalog.Interfaces.REST.Resources;

namespace Kipu.API.Logistics.MachineryCatalog.Interfaces.REST.Transform;

public static class UpdatePartialMachineryCatalogCommandFromResourceAssembler
{
    public static UpdateMachineryCatalogPartialCommand ToCommandFromResource(string id, UpdatePartialMachineryCatalogResource resource) =>
        new(id, resource.Name, resource.Brand, resource.Model, resource.SerialNumber, resource.AcquisitionDate);
}
