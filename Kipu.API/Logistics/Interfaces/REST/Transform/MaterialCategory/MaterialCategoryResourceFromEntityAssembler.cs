using MaterialCategoryEntity = Kipu.API.Logistics.Domain.Model.Aggregates.MaterialCategory;
using Kipu.API.Logistics.Interfaces.REST.Resources.MaterialCategory;

namespace Kipu.API.Logistics.Interfaces.REST.Transform.MaterialCategory;

public static class MaterialCategoryResourceFromEntityAssembler
{
    public static MaterialCategoryResource ToResourceFromEntity(MaterialCategoryEntity entity) =>
        new(entity.Id, entity.Name.Value, entity.Description ?? string.Empty, entity.IsActive);
}