using Kipu.API.Logistics.Domain.Model.Commands;
using Kipu.API.Logistics.Domain.Model.ValueObjects;
using Kipu.API.Logistics.Domain.Model.ValueObjects.External;
using Kipu.API.Logistics.Interfaces.REST.Resources.MaterialRequest;

namespace Kipu.API.Logistics.Interfaces.REST.Transform.MaterialRequest;

public static class UpdateMaterialRequestCommandFromResourceAssembler
{
    public static UpdateMaterialRequestCommand ToCommandFromResource(int id, UpdateMaterialRequestResource resource) =>
        new(
            id,
            resource.Deadline,
            Enum.Parse<RequestPriority>(resource.RequestPriority),
            resource.DeliveryLocation,
            resource.BudgetLineId.HasValue ? new BudgetLineId(resource.BudgetLineId.Value) : null,
            resource.Purpose,
            resource.AdditionalNotes ?? string.Empty,
            resource.Items.Select(i => new MaterialRequestItemCommand(
                new MaterialCatalogId(i.MaterialCatalogId),
                new SupplierId(i.SupplierId),
                i.Quantity,
                i.UnitPrice
            )).ToList()
        );
}
