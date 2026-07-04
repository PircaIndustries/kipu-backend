using Kipu.API.Logistics.Domain.Model.ValueObjects;

namespace Kipu.API.Logistics.Domain.Model.Queries;

public record GetSupplierOffersByMaterialIdQuery(MaterialCatalogId MaterialCatalogId);
