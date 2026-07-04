using Kipu.API.Logistics.Domain.Model.Commands;
using Kipu.API.Logistics.Domain.Model.ValueObjects;

namespace Kipu.API.Logistics.Domain.Model.Aggregates;

public class SupplierOffer
{
    protected SupplierOffer()
    {
    }

    public SupplierOffer(CreateSupplierOfferCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);
        SupplierId = command.SupplierId;
        MaterialCatalogId = command.MaterialCatalogId;
        UnitPrice = command.UnitPrice;
    }

    public void Update(UpdateSupplierOfferCommand command)
    {
        SupplierId = command.SupplierId;
        MaterialCatalogId = command.MaterialCatalogId;
        UnitPrice = command.UnitPrice;
    }

    public int Id { get; private set; }
    public SupplierId SupplierId { get; private set; }
    public MaterialCatalogId MaterialCatalogId { get; private set; }
    public decimal UnitPrice { get; private set; }
}
