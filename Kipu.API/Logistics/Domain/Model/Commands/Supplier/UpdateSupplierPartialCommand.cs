using Kipu.API.Logistics.Domain.Model.ValueObjects;

namespace Kipu.API.Logistics.Domain.Model.Commands;

public record UpdateSupplierPartialCommand(
    int Id,
    Ruc? Ruc,
    SocialReason? SocialReason,
    string? Contact = null,
    Phone? Phone = null,
    Email? Email = null,
    string? PaymentTerms = null,
    bool? IsActive = null
);