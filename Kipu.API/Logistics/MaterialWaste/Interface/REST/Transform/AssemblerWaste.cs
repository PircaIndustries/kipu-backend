using Kipu.API.Logistics.MaterialWaste.Domain.Model.Commands;
using Kipu.API.Logistics.MaterialWaste.Interface.REST.Resources;

namespace Kipu.API.Logistics.MaterialWaste.Interface.REST.Transform;

public static class MaterialWasteAssembler
{
    public static CreateMaterialWasteCommand ToCommandFromResource(CreateMaterialWasteResource resource) =>
        new(resource.ProjectId, resource.MaterialId, resource.Quantity, 
            resource.ClassificationType, resource.Description, resource.ReportedBy, resource.PhotoUrl);

    public static UpdateMaterialWasteCommand ToCommandFromResource(string id, UpdateMaterialWasteResource resource) =>
        new(id, resource.Quantity, resource.ClassificationType, resource.Description);

    public static MaterialWasteResource ToResourceFromEntity(Domain.Model.Aggregates.MaterialWaste entity) =>
        new(entity.Id.Value, entity.ProjectId, entity.MaterialId, entity.Quantity, 
            entity.ClassificationType, entity.Date, entity.Description, entity.ReportedBy, entity.PhotoUrl);
}