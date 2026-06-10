using Kipu.API.Logistics.Domain.Model.Commands;
using Kipu.API.Logistics.Domain.Model.ValueObjects;
using Kipu.API.Logistics.Domain.Model.ValueObjects.External;

namespace Kipu.API.Logistics.Domain.Model.Aggregates;

public partial class MaterialInventory
{
    protected MaterialInventory()
    {
        ProjectId = null!;
        MaterialId = null!;
        CurrentStock = null!;
        MinimumStock = null!;
    }

    public MaterialInventory(CreateMaterialInventoryCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);
        ProjectId = command.ProjectId;
        MaterialId = command.MaterialId;
        CurrentStock = command.CurrentStock;
        MinimumStock = command.MinimumStock ?? new Quantity(0);
        Location = command.Location;
    }
    public int Id { get; private set; }
    public ProjectId ProjectId { get; private set; }
    public MaterialId MaterialId { get; private set; }
    public Quantity CurrentStock { get; private set; }
    public Quantity MinimumStock { get; private set; }
    public String Location { get; private set; }
}