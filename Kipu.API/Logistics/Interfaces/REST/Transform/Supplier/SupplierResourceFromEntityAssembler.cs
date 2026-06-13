
using Kipu.API.Logistics.Interfaces.REST.Resources.Supplier;
using SupplierEntity = Kipu.API.Logistics.Domain.Model.Aggregates.Supplier;
namespace Kipu.API.Logistics.Interfaces.REST.Transform.Supplier;

public static class SupplierResourceFromEntityAssembler
{
    public static SupplierResource ToResourceFromEntity(SupplierEntity entity) =>
        new(
            entity.Id,
            entity.Ruc.Value,
            entity.SocialReason.Value,
            entity.Contact,
            entity.Phone.Value,
            entity.Email.Value,
            entity.PaymentTerms,
            entity.IsActive
        );
}