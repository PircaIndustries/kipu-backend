using Kipu.API.Logistics.Domain.Model.ValueObjects;

namespace Kipu.API.Logistics.Domain.Model.Commands;

public record CreateSupplierCommand(Ruc Ruc, SocialReason SocialReason, string Contact, Phone Phone, Email Email,
String PaymentTerms, Boolean IsActive);