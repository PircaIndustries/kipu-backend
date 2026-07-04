using Kipu.API.Logistics.Domain.Model.ValueObjects;

namespace Kipu.API.Logistics.Domain.Model.Commands;

public record UpdateSupplierOfferCommand(int Id, SupplierId SupplierId, MaterialCatalogId MaterialCatalogId, decimal UnitPrice);
