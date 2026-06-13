using Kipu.API.Logistics.Domain.Model.Commands;
using Kipu.API.Logistics.Domain.Model.ValueObjects;
using Kipu.API.Logistics.Interfaces.REST.Resources.Supplier;

namespace Kipu.API.Logistics.Interfaces.REST.Transform.Supplier;

public static class UpdatePartialSupplierCommandFromResourceAssembler
{
    public static UpdateSupplierPartialCommand ToCommandFromResource(int id, UpdatePartialSupplierResource resource)
    {
        return new UpdateSupplierPartialCommand(
            id,
            resource.Ruc is not null ? new Ruc(resource.Ruc) : null,
            resource.SocialReason is not null ? new SocialReason(resource.SocialReason) : null,
            resource.Contact,
            resource.Phone is not null ? new Phone(resource.Phone) : null,
            resource.Email is not null ? new Email(resource.Email) : null,
            resource.PaymentTerms,
            resource.IsActive
        );
    }
}