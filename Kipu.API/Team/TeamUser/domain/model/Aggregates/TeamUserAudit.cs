using Kipu.API.Shared.Domain.Model;

namespace Kipu.API.Team.TeamUser.domain.model.Aggregates;

public partial class TeamUser : IAuditableEntity
{
    public DateTimeOffset? CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
}