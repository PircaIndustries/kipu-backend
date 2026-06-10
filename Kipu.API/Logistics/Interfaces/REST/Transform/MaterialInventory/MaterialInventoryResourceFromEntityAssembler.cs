using Kipu.API.Logistics.Domain.Model.Aggregates;
using Kipu.API.Logistics.Interfaces.REST.Resources;

namespace Kipu.API.Logistics.Interfaces.REST.Transform;

public static class MaterialInventoryResourceFromEntityAssembler
{
    public static MaterialInventoryResource ToResourceFromEntity(MaterialInventory entity) =>
        new(entity.Id, entity.ProjectId.Value, entity.MaterialCatalogId.Value, entity.CurrentStock.Value, entity.MinimumStock.Value, entity.Location.Value);
}