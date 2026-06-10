using Kipu.API.Shared.Domain.Model;

namespace Kipu.API.Logistics.Domain.Model.Aggregates.Audit;

public partial class Supplier : IAuditableEntity
{
    public DateTimeOffset? CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
}