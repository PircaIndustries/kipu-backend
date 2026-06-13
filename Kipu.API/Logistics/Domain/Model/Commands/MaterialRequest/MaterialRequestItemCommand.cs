using Kipu.API.Logistics.Domain.Model.ValueObjects;

namespace Kipu.API.Logistics.Domain.Model.Commands;

public record MaterialRequestItemCommand(
    MaterialCatalogId MaterialCatalogId,
    SupplierId SupplierId,
    decimal Quantity,
    decimal UnitPrice);
