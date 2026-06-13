using Kipu.API.Logistics.Domain.Model.Commands;
using Kipu.API.Logistics.Domain.Model.ValueObjects;
using Kipu.API.Logistics.Domain.Model.ValueObjects.External;

namespace Kipu.API.Logistics.Domain.Model.Aggregates;

public class MaterialRequest
{
    protected MaterialRequest()
    {
        Items = new List<MaterialRequestItem>();
    }

    public MaterialRequest(CreateMaterialRequestCommand command) : this()
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
        Items = command.Items
            .Select(i => new MaterialRequestItem(i))
            .ToList();
    }

    public void Update(UpdateMaterialRequestCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);
        if (RequestStatus == RequestStatus.Accepted)
            throw new InvalidOperationException("Cannot update a request that has been accepted.");
        if (RequestStatus == RequestStatus.Refused)
            throw new InvalidOperationException("Cannot update a request that has been refused.");
        Deadline = command.Deadline;
        RequestPriority = command.RequestPriority;
        DeliveryLocation = command.DeliveryLocation;
        BudgetLineId = command.BudgetLineId;
        Purpose = command.Purpose;
        AdditionalNotes = command.AdditionalNotes;
        Items = command.Items
            .Select(i => new MaterialRequestItem(i))
            .ToList();
    }

    public void UpdatePartial(UpdateMaterialRequestPartialCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);
        if (RequestStatus == RequestStatus.Accepted)
            throw new InvalidOperationException("Cannot update a request that has been accepted.");
        if (RequestStatus == RequestStatus.Refused)
            throw new InvalidOperationException("Cannot update a request that has been refused.");
        if (command.Deadline.HasValue)
            Deadline = command.Deadline.Value;
        if (command.RequestPriority.HasValue)
            RequestPriority = command.RequestPriority.Value;
        if (command.DeliveryLocation is not null)
            DeliveryLocation = command.DeliveryLocation;
        if (command.BudgetLineId is not null)
            BudgetLineId = command.BudgetLineId;
        if (command.Purpose is not null)
            Purpose = command.Purpose;
        if (command.AdditionalNotes is not null)
            AdditionalNotes = command.AdditionalNotes;
        if (command.Items is not null)
        {
            foreach (var itemCommand in command.Items)
            {
                if (itemCommand.Id.HasValue && itemCommand.Id.Value > 0)
                {
                    var existingItem = Items.FirstOrDefault(i => i.Id == itemCommand.Id.Value);
                    if (existingItem is not null)
                        existingItem.UpdatePartial(itemCommand);
                }
                else
                {
                    Items.Add(new MaterialRequestItem(
                        new MaterialRequestItemCommand(
                            itemCommand.MaterialCatalogId ?? throw new ArgumentException("MaterialCatalogId is required for new items"),
                            itemCommand.SupplierId ?? throw new ArgumentException("SupplierId is required for new items"),
                            itemCommand.Quantity ?? throw new ArgumentException("Quantity is required for new items"),
                            itemCommand.UnitPrice ?? throw new ArgumentException("UnitPrice is required for new items")
                        )
                    ));
                }
            }
        }
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
    public ICollection<MaterialRequestItem> Items { get; private set; }
}