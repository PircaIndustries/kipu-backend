using Kipu.API.Logistics.Interfaces.REST.Resources.MaterialRequest;
using RequestEntity = Kipu.API.Logistics.Domain.Model.Aggregates.MaterialRequest;

namespace Kipu.API.Logistics.Interfaces.REST.Transform.MaterialRequest;

public static class MaterialRequestResourceFromEntityAssembler
{
    public static MaterialRequestResource ToResourceFromEntity(RequestEntity entity) =>
        new(
            entity.Id,
            entity.Deadline,
            entity.RequestStatus.ToString(),
            entity.RequestPriority.ToString(),
            entity.DeliveryLocation,
            entity.BudgetLineId.Value,
            entity.Purpose,
            entity.AdditionalNotes,
            entity.RequestedBy.Value,
            entity.Items.Select(i => new MaterialRequestItemResource(
                i.Id,
                i.MaterialCatalogId.Value,
                i.SupplierId.Value,
                i.Quantity,
                i.UnitPrice
            )).ToList()
        );
}
