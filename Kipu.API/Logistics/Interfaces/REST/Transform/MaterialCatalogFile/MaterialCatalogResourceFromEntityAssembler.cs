using MaterialCatalogEntity = Kipu.API.Logistics.Domain.Model.Aggregates.MaterialCatalog;
using Kipu.API.Logistics.Interfaces.REST.Resources;
namespace Kipu.API.Logistics.Interfaces.REST.Transform;

public class MaterialCatalogResourceFromEntityAssembler
{
    public static MaterialCatalogResource ToResourceFromEntity(MaterialCatalogEntity entity) =>
        new(entity.Id, entity.Name.ToString(), entity.CategoryId.Value, entity.MeasureUnit);
}
