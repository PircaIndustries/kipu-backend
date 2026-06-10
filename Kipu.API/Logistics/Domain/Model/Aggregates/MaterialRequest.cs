using Kipu.API.Logistics.Domain.Model.Commands;
using Kipu.API.Logistics.Domain.Model.ValueObjects;
using Kipu.API.Logistics.Domain.Model.ValueObjects.External;

namespace Kipu.API.Logistics.Domain.Model.Aggregates;

public class MaterialRequest
{
    protected MaterialRequest()
    {

    }

    public MaterialRequest(CreateMaterialRequestCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);
        Deadline = command.Deadline;
        RequestStatus = RequestStatus.Pending;
        RequestPriority = command.RequestPriority;
        DeliveryLocation = command.DeliveryLocation;
        BudgetLineId = command.BudgetLineId;
        Purpose = command.Purpose;
        AdditionalNotes = command.AdditionalNotes;
        RequestedBy = command.RequestedBy;
    }

    public int Id { get; private set; }
    public DateTime Deadline { get; private set; }
    public RequestStatus RequestStatus { get; private set; }
    public RequestPriority RequestPriority { get; private set; }
    public String DeliveryLocation { get; private set; }
    public BudgetLineId BudgetLineId { get; private set; }
    public String Purpose { get; private set; }
    public String AdditionalNotes { get; private set; }
    public UserId RequestedBy { get; private set; }
}