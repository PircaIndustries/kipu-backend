using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;

namespace Kipu.API.Logistics.Interfaces.REST.Resources.Supplier;

/// <summary>
/// Request payload to create a new supplier
/// </summary>
/// <param name="Ruc">The supplier's tax identification number (RUC)</param>
/// <param name="SocialReason">The supplier's business name</param>
/// <param name="Contact">The main contact person name</param>
/// <param name="Phone">The supplier's contact phone number</param>
/// <param name="Email">The supplier's email address</param>
/// <param name="PaymentTerms">The payment terms agreed with the supplier</param>
[SwaggerSchema(Description = "Request payload to create a new supplier")]
public record CreateSupplierResource(
    [Required]
    [StringLength(11, MinimumLength = 11)]
    [SwaggerParameter(Description = "The supplier's tax identification number (RUC) - 11 digits")] 
    string Ruc,
    
    [Required]
    [StringLength(200, MinimumLength = 3)]
    [SwaggerParameter(Description = "The supplier's business name")] 
    string SocialReason,
    
    [Required]
    [StringLength(100)]
    [SwaggerParameter(Description = "The main contact person name")] 
    string Contact,
    
    [Required]
    [Phone]
    [StringLength(20)]
    [SwaggerParameter(Description = "The supplier's contact phone number")] 
    string Phone,
    
    [Required]
    [EmailAddress]
    [StringLength(100)]
    [SwaggerParameter(Description = "The supplier's email address")] 
    string Email,
    
    [Required]
    [StringLength(500)]
    [SwaggerParameter(Description = "The payment terms agreed with the supplier")] 
    string PaymentTerms)
    ;