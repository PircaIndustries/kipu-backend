using Kipu.API.Logistics.Domain.Model.Commands;
using Kipu.API.Logistics.Domain.Model.ValueObjects;
using Kipu.API.Logistics.Interfaces.REST.Resources.SupplierOffer;

namespace Kipu.API.Logistics.Interfaces.REST.Transform.SupplierOffer;

public static class CreateSupplierOfferCommandFromResourceAssembler
{
    public static CreateSupplierOfferCommand ToCommandFromResource(CreateSupplierOfferResource resource) =>
        new(new SupplierId(resource.SupplierId), new MaterialCatalogId(resource.MaterialId), resource.UnitPrice);
}
