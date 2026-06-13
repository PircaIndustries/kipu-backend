using Kipu.API.Logistics.Domain.Model.Commands;
using Kipu.API.Logistics.Domain.Model.ValueObjects;
using Kipu.API.Logistics.Domain.Model.ValueObjects.External;
using Kipu.API.Logistics.Interfaces.REST.Resources.MaterialRequest;

namespace Kipu.API.Logistics.Interfaces.REST.Transform.MaterialRequest;

public static class CreateMaterialRequestCommandFromResourceAssembler
{
    public static CreateMaterialRequestCommand ToCommandFromResource(CreateMaterialRequestResource resource) =>
        new(
            resource.Deadline,
            Enum.Parse<RequestPriority>(resource.RequestPriority),
            resource.DeliveryLocation,
            new BudgetLineId(resource.BudgetLineId),
            resource.Purpose,
            resource.AdditionalNotes ?? string.Empty,
            new UserId(resource.RequestedBy),
            resource.Items.Select(i => new MaterialRequestItemCommand(
                new MaterialCatalogId(i.MaterialCatalogId),
                new SupplierId(i.SupplierId),
                i.Quantity,
                i.UnitPrice
            )).ToList()
        );
}
