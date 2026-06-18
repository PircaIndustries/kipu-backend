using Kipu.API.Logistics.Domain.Model.ValueObjects;
using Kipu.API.Logistics.Domain.Model.ValueObjects.External;

namespace Kipu.API.Logistics.Domain.Model.Commands;

public record CreateMaterialRequestCommand(
    DateTime Deadline,
    RequestPriority RequestPriority,
    string DeliveryLocation,
    BudgetLineId? BudgetLineId,
    string Purpose,
    string AdditionalNotes,
    UserId RequestedBy,
    List<MaterialRequestItemCommand> Items);
