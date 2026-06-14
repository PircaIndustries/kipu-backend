using Kipu.API.Shared.Domain.Model;

namespace Kipu.API.Team.TeamWorker.Domain.Model.Aggregates;

public partial class TeamWorker : IAuditableEntity
{
    public DateTimeOffset? CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
}