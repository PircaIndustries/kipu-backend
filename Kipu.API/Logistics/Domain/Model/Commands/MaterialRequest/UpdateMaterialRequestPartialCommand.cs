using Kipu.API.Logistics.Domain.Model.ValueObjects;
using Kipu.API.Logistics.Domain.Model.ValueObjects.External;

namespace Kipu.API.Logistics.Domain.Model.Commands;

public record UpdateMaterialRequestPartialCommand(
    int Id,
    DateTime? Deadline = null,
    RequestPriority? RequestPriority = null,
    string? DeliveryLocation = null,
    BudgetLineId? BudgetLineId = null,
    string? Purpose = null,
    string? AdditionalNotes = null,
    List<UpdatePartialMaterialRequestItemCommand>? Items = null);
