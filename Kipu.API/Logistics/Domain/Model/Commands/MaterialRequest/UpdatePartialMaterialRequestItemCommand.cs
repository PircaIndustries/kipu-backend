using Kipu.API.Logistics.Domain.Model.ValueObjects;

namespace Kipu.API.Logistics.Domain.Model.Commands;

public record UpdatePartialMaterialRequestItemCommand(
    int? Id,
    MaterialCatalogId? MaterialCatalogId = null,
    SupplierId? SupplierId = null,
    decimal? Quantity = null,
    decimal? UnitPrice = null);
