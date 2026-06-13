using Kipu.API.Logistics.Domain.Model.Commands;
using Kipu.API.Logistics.Domain.Model.ValueObjects;

namespace Kipu.API.Logistics.Domain.Model.Aggregates;

public class Supplier
{
    protected Supplier()
    {
        Ruc = null!;
        SocialReason = null!;
        Phone = null!;
        Email = null!;
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
    public void Update(UpdateSupplierCommand command)
    {
        Ruc = command.Ruc;
        SocialReason = command.SocialReason;
        Contact = command.Contact;
        Phone = command.Phone;
        Email = command.Email;
        PaymentTerms = command.PaymentTerms;
        IsActive = command.IsActive;
    }
    public void UpdatePartial(UpdateSupplierPartialCommand command)
    {
        if (command.Ruc is not null)
            Ruc = command.Ruc;
        
        if (command.SocialReason is not null)
            SocialReason = command.SocialReason;
        
        if (command.Contact is not null)
            Contact = command.Contact;
        
        if (command.Phone is not null)
            Phone = command.Phone;
        
        if (command.Email is not null)
            Email = command.Email;
        
        if (command.PaymentTerms is not null)
            PaymentTerms = command.PaymentTerms;
        
        if (command.IsActive.HasValue)
            IsActive = command.IsActive.Value;
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