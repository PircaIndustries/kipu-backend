using Kipu.API.Logistics.Domain.Model.Commands;
using Kipu.API.Logistics.Domain.Model.ValueObjects;
using Kipu.API.Logistics.Interfaces.REST.Resources;

namespace Kipu.API.Logistics.Interfaces.REST.Transform.MaterialCatalog;

public static class CreateMaterialCatalogCommandFromResourceAssembler
{
    public static CreateMaterialCatalogCommand ToCommandFromResource(CreateMaterialCatalogResource resource) =>
        new(new Name(resource.Name), new CategoryId(resource.CategoryId), resource.MeasureUnit);
}

