using Kipu.API.Logistics.Domain.Model.Commands;
using Kipu.API.Logistics.Domain.Model.ValueObjects;
using Kipu.API.Logistics.Interfaces.REST.Resources.MaterialCategory;

namespace Kipu.API.Logistics.Interfaces.REST.Transform.MaterialCategory;

public static class CreateMaterialCategoryCommandFromResourceAssembler
{
    public static CreateMaterialCategoryCommand ToCommandFromResource(CreateMaterialCategoryResource resource) =>
        new(new Name(resource.Name), resource.Description, resource.IsActive);
}