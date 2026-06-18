using Kipu.API.Logistics.Interfaces.REST.Resources.SupplierOffer;
using SupplierOfferEntity = Kipu.API.Logistics.Domain.Model.Aggregates.SupplierOffer;

namespace Kipu.API.Logistics.Interfaces.REST.Transform.SupplierOffer;

public static class SupplierOfferResourceFromEntityAssembler
{
    public static SupplierOfferResource ToResourceFromEntity(SupplierOfferEntity entity) =>
        new(entity.Id, entity.SupplierId.Value, entity.MaterialCatalogId.Value, entity.UnitPrice);
}
