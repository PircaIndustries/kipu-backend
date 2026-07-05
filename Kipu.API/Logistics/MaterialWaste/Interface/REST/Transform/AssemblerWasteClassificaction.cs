using Kipu.API.Logistics.MaterialWaste.Domain.Model.Commands;
using Kipu.API.Logistics.MaterialWaste.Interface.REST.Resources;

namespace Kipu.API.Logistics.MaterialWaste.Interface.REST.Transform;

public static class WasteClassificationAssembler
{
    public static CreateWasteClassificationCommand ToCommandFromResource(CreateWasteClassificationResource resource) =>
        new(resource.Name);

    public static WasteClassificationResource ToResourceFromEntity(Domain.Model.Aggregates.WasteClassification entity) =>
        new(entity.Id.Value, entity.Name);
}