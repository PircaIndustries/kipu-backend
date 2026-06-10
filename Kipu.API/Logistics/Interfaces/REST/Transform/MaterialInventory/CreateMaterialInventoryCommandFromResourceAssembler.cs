using Kipu.API.Logistics.Domain.Model.Commands;
using Kipu.API.Logistics.Domain.Model.ValueObjects;
using Kipu.API.Logistics.Domain.Model.ValueObjects.External;
using Kipu.API.Logistics.Interfaces.REST.Resources;

namespace Kipu.API.Logistics.Interfaces.REST.Transform;

public static class CreateMaterialInventoryCommandFromResourceAssembler
{
    public static CreateMaterialInventoryCommand ToCommandFromResource(CreateMaterialInventoryResource resource) =>
        new(new ProjectId(resource.ProjectId), new MaterialCatalogId(resource.MaterialId), new Quantity(resource.CurrentStock), new Quantity(resource.MinimumStock), new WarehouseLocation(resource.Location));
}