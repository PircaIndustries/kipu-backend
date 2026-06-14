using Kipu.API.Shared.Domain.Model;

namespace Kipu.API.Document.Domain.Model.Aggregates;

public partial class Document : IAuditableEntity
{
    public DateTimeOffset? CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
}