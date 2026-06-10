using Kipu.API.Logistics.Domain.Model.ValueObjects;
using Kipu.API.Logistics.Domain.Model.ValueObjects.External;

namespace Kipu.API.Logistics.Domain.Model.Commands;

public record CreateMaterialRequestCommand(DateTime Deadline, RequestStatus? RequestStatus,
    RequestPriority RequestPriority, String DeliveryLocation, BudgetLineId BudgetLineId,
    String Purpose, String AdditionalNotes, UserId RequestedBy);