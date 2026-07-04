using Kipu.API.Logistics.Domain.Model.Commands;
using Kipu.API.Logistics.Domain.Model.ValueObjects;
using Kipu.API.Logistics.Interfaces.REST.Resources.SupplierOffer;

namespace Kipu.API.Logistics.Interfaces.REST.Transform.SupplierOffer;

public static class UpdateSupplierOfferCommandFromResourceAssembler
{
    public static UpdateSupplierOfferCommand ToCommandFromResource(int id, UpdateSupplierOfferResource resource) =>
        new(id, new SupplierId(resource.SupplierId), new MaterialCatalogId(resource.MaterialId), resource.UnitPrice);
}
