namespace Kipu.API.Document.Domain.Model.ValueObjects;

public record DocumentId(string Value)
{
    public DocumentId() : this($"doc-{Guid.NewGuid().ToString("N")[..8]}") { }  
};