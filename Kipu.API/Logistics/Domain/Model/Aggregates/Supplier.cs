using Kipu.API.Logistics.Domain.Model.Commands;
using Kipu.API.Logistics.Domain.Model.ValueObjects;

namespace Kipu.API.Logistics.Domain.Model.Aggregates;

public class Supplier
{
    protected Supplier()
    {
        
    }

    public Supplier(CreateSupplierCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);
        Ruc = command.Ruc;
        SocialReason = command.SocialReason;
        Contact = command.Contact;
        Phone = command.Phone;
        Email = command.Email;
        PaymentTerms = command.PaymentTerms;
        IsActive = true;
    }
    public int Id { get; private set; }
    public Ruc Ruc { get; private set; }
    public SocialReason SocialReason { get; private set; }
    public String Contact { get; private set; }
    public Phone Phone { get; private set; }
    public Email Email { get; private set; }
    public String PaymentTerms { get; private set; }
    public Boolean IsActive { get; private set; }
}