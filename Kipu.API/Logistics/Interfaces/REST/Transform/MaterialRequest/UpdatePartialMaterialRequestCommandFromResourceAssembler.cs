using Kipu.API.Logistics.Domain.Model.Commands;
using Kipu.API.Logistics.Domain.Model.ValueObjects;
using Kipu.API.Logistics.Domain.Model.ValueObjects.External;
using Kipu.API.Logistics.Interfaces.REST.Resources.MaterialRequest;

namespace Kipu.API.Logistics.Interfaces.REST.Transform.MaterialRequest;

public static class UpdatePartialMaterialRequestCommandFromResourceAssembler
{
    public static UpdateMaterialRequestPartialCommand ToCommandFromResource(int id, UpdatePartialMaterialRequestResource resource)
    {
        List<UpdatePartialMaterialRequestItemCommand>? items = null;
        if (resource.Items is not null)
        {
            items = resource.Items.Select(i => new UpdatePartialMaterialRequestItemCommand(
                i.Id,
                i.MaterialCatalogId.HasValue ? new MaterialCatalogId(i.MaterialCatalogId.Value) : null,
                i.SupplierId.HasValue ? new SupplierId(i.SupplierId.Value) : null,
                i.Quantity,
                i.UnitPrice
            )).ToList();
        }

        return new UpdateMaterialRequestPartialCommand(
            id,
            resource.Deadline,
            resource.RequestPriority is not null ? Enum.Parse<RequestPriority>(resource.RequestPriority) : null,
            resource.DeliveryLocation,
            resource.BudgetLineId.HasValue ? new BudgetLineId(resource.BudgetLineId.Value) : null,
            resource.Purpose,
            resource.AdditionalNotes,
            items,
            resource.RequestStatus is not null ? Enum.Parse<RequestStatus>(resource.RequestStatus) : null
        );
    }
}
