using Kipu.API.Logistics.Domain.Model.ValueObjects;
using Kipu.API.Logistics.Domain.Model.ValueObjects.External;

namespace Kipu.API.Logistics.Domain.Model.Commands;

public record CreateMaterialInventoryCommand(ProjectId ProjectId, MaterialId MaterialId, Quantity CurrentStock, Quantity? MinimumStock, String Location);