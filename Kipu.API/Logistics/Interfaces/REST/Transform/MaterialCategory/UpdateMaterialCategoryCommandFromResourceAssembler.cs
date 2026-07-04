using Kipu.API.Logistics.Domain.Model.Commands;
using Kipu.API.Logistics.Domain.Model.ValueObjects;
using Kipu.API.Logistics.Interfaces.REST.Resources.MaterialCategory;

namespace Kipu.API.Logistics.Interfaces.REST.Transform.MaterialCategory;

public static class UpdateMaterialCategoryCommandFromResourceAssembler
{
    public static UpdateMaterialCategoryCommand ToCommandFromResource(int id, UpdateMaterialCategoryResource resource) =>
        new(id, new Name(resource.Name), resource.Description ?? string.Empty, resource.IsActive);
}
