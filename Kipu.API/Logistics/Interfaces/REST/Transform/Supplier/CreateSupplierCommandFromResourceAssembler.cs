using Kipu.API.Logistics.Domain.Model.Commands;
using Kipu.API.Logistics.Domain.Model.ValueObjects;
using Kipu.API.Logistics.Interfaces.REST.Resources.Supplier;

namespace Kipu.API.Logistics.Interfaces.REST.Transform.Supplier;

public static class CreateSupplierCommandFromResourceAssembler
{
    public static CreateSupplierCommand ToCommandFromResource(CreateSupplierResource resource) =>
        new(
            new Ruc(resource.Ruc),
            new SocialReason(resource.SocialReason),
            resource.Contact,
            new Phone(resource.Phone),
            new Email(resource.Email),
            resource.PaymentTerms
        );
}