using System.Text.Json.Serialization;
using Kipu.API.Logistics.Domain.Model.Commands;
using Kipu.API.Logistics.Domain.Model.ValueObjects;

namespace Kipu.API.Logistics.Domain.Model.Aggregates;

public class MaterialRequestItem
{
    protected MaterialRequestItem()
    {
    }

    public MaterialRequestItem(MaterialRequestItemCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);
        MaterialCatalogId = command.MaterialCatalogId;
        SupplierId = command.SupplierId;
        Quantity = command.Quantity;
        UnitPrice = command.UnitPrice;
    }

    public void UpdatePartial(UpdatePartialMaterialRequestItemCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);
        if (command.MaterialCatalogId is not null)
            MaterialCatalogId = command.MaterialCatalogId;
        if (command.SupplierId is not null)
            SupplierId = command.SupplierId;
        if (command.Quantity.HasValue)
            Quantity = command.Quantity.Value;
        if (command.UnitPrice.HasValue)
            UnitPrice = command.UnitPrice.Value;
    }

    public int Id { get; private set; }
    public int MaterialRequestId { get; private set; }

    [JsonIgnore]
    public MaterialRequest? MaterialRequest { get; private set; }

    public MaterialCatalogId MaterialCatalogId { get; private set; }
    public SupplierId SupplierId { get; private set; }
    public decimal Quantity { get; private set; }
    public decimal UnitPrice { get; private set; }
}
