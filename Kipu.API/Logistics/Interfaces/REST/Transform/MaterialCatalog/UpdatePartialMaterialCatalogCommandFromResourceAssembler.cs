using Kipu.API.Logistics.Domain.Model.Commands;
using Kipu.API.Logistics.Domain.Model.ValueObjects;
using Kipu.API.Logistics.Interfaces.REST.Resources;

namespace Kipu.API.Logistics.Interfaces.REST.Transform.MaterialCatalog;

public static class UpdatePartialMaterialCatalogCommandFromResourceAssembler
{
    public static UpdateMaterialCatalogPartialCommand ToCommandFromResource(int id, UpdatePartialMaterialCatalogResource resource)
    {
        return new UpdateMaterialCatalogPartialCommand(
            id,
            resource.Name is not null ? new Name(resource.Name) : null,
            resource.CategoryId.HasValue ? new CategoryId(resource.CategoryId.Value) : null,
            resource.MeasureUnit
        );
    }
}
