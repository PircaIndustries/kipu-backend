using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;

namespace Kipu.API.Logistics.Interfaces.REST.Resources.Supplier;

/// <summary>
/// Request payload to partially update an existing supplier
/// </summary>
/// <param name="Ruc">The supplier's tax identification number (RUC) - 11 digits (optional)</param>
/// <param name="SocialReason">The supplier's business name (optional)</param>
/// <param name="Contact">The main contact person name (optional)</param>
/// <param name="Phone">The supplier's contact phone number (optional)</param>
/// <param name="Email">The supplier's email address (optional)</param>
/// <param name="PaymentTerms">The payment terms agreed with the supplier (optional)</param>
/// <param name="IsActive">Indicates whether the supplier is active (optional)</param>
[SwaggerSchema(Description = "Request payload to partially update an existing supplier")]
public record UpdatePartialSupplierResource(
    [StringLength(11, MinimumLength = 11)]
    [SwaggerParameter(Description = "The supplier's tax identification number (RUC) - 11 digits (optional)")]
    string? Ruc,

    [StringLength(200, MinimumLength = 3)]
    [SwaggerParameter(Description = "The supplier's business name (optional)")]
    string? SocialReason,

    [StringLength(100)]
    [SwaggerParameter(Description = "The main contact person name (optional)")]
    string? Contact,

    [Phone]
    [StringLength(20)]
    [SwaggerParameter(Description = "The supplier's contact phone number (optional)")]
    string? Phone,

    [EmailAddress]
    [StringLength(100)]
    [SwaggerParameter(Description = "The supplier's email address (optional)")]
    string? Email,

    [StringLength(500)]
    [SwaggerParameter(Description = "The payment terms agreed with the supplier (optional)")]
    string? PaymentTerms,

    [SwaggerParameter(Description = "Indicates whether the supplier is active (optional)")]
    bool? IsActive
);