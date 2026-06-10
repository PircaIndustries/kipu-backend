using Kipu.API.Logistics.Domain.Model.ValueObjects;
using Kipu.API.Logistics.Domain.Model.ValueObjects.External;

namespace Kipu.API.Logistics.Domain.Model.Commands;

public record CreateMaterialInventoryCommand(ProjectId ProjectId, MaterialCatalogId MaterialCatalogId, Quantity CurrentStock, Quantity? MinimumStock, WarehouseLocation Location);