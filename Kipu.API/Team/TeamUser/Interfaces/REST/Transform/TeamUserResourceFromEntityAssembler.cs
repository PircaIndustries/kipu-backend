using Kipu.API.Team.TeamUser.Interfaces.REST.Resources;
using TeamUserAggregate = Kipu.API.Team.TeamUser.domain.model.Aggregates.TeamUser;

namespace Kipu.API.Team.TeamUser.Interfaces.REST.Transform;

public static class TeamUserResourceFromEntityAssembler
{
    public static TeamUserResource ToResourceFromEntity(TeamUserAggregate entity) =>
        new(entity.Id.Value, entity.UserId, entity.FullName, entity.Email.Address, entity.Role, entity.IsActive, entity.ProjectId);
}
