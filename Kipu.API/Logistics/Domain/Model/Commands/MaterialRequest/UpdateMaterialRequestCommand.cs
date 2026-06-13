using Kipu.API.Logistics.Domain.Model.ValueObjects;
using Kipu.API.Logistics.Domain.Model.ValueObjects.External;

namespace Kipu.API.Logistics.Domain.Model.Commands;

public record UpdateMaterialRequestCommand(
    int Id,
    DateTime Deadline,
    RequestPriority RequestPriority,
    string DeliveryLocation,
    BudgetLineId BudgetLineId,
    string Purpose,
    string AdditionalNotes,
    List<MaterialRequestItemCommand> Items);
