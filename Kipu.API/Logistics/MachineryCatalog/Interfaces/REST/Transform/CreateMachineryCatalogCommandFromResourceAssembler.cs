using Kipu.API.Logistics.MachineryCatalog.Domain.Model.Commands;
using Kipu.API.Logistics.MachineryCatalog.Interfaces.REST.Resources;

namespace Kipu.API.Logistics.MachineryCatalog.Interfaces.REST.Transform;

public static class CreateMachineryCatalogCommandFromResourceAssembler
{
    public static CreateMachineryCatalogCommand ToCommandFromResource(CreateMachineryCatalogResource resource) =>
        new(resource.Name, resource.Brand, resource.Model, resource.SerialNumber, resource.AcquisitionDate);
}
