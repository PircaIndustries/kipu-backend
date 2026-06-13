using Swashbuckle.AspNetCore.Annotations;

namespace Kipu.API.Logistics.Interfaces.REST.Resources.Supplier;

/// <summary>
/// Supplier resource returned by the API
/// </summary>
/// <param name="Id">The server-generated ID of the supplier</param>
/// <param name="Ruc">The supplier's tax identification number (RUC)</param>
/// <param name="SocialReason">The supplier's business name</param>
/// <param name="Contact">The main contact person name</param>
/// <param name="Phone">The supplier's contact phone number</param>
/// <param name="Email">The supplier's email address</param>
/// <param name="PaymentTerms">The payment terms agreed with the supplier</param>
/// <param name="IsActive">Indicates whether the supplier is active</param>
[SwaggerSchema(Description = "A supplier resource")]
public record SupplierResource(
    [SwaggerParameter(Description = "The server-generated ID of the supplier")] 
    int Id,
    
    [SwaggerParameter(Description = "The supplier's tax identification number (RUC)")] 
    string Ruc,
    
    [SwaggerParameter(Description = "The supplier's business name")] 
    string SocialReason,
    
    [SwaggerParameter(Description = "The main contact person name")] 
    string Contact,
    
    [SwaggerParameter(Description = "The supplier's contact phone number")] 
    string Phone,
    
    [SwaggerParameter(Description = "The supplier's email address")] 
    string Email,
    
    [SwaggerParameter(Description = "The payment terms agreed with the supplier")] 
    string PaymentTerms,
    
    [SwaggerParameter(Description = "Indicates whether the supplier is active")] 
    bool IsActive);