using Kipu.API.Team.TeamUser.Interfaces.REST.Resources;

namespace Kipu.API.Team.TeamUser.Interfaces.REST.Transform;

public static class TeamUserResourceFromEntityAssembler
{
    public static TeamUserResource ToResourceFromEntity(Domain.Model.Aggregates.TeamUser entity) =>
        new(entity.Id.Value, entity.UserId, entity.FullName, entity.Email.Address, entity.Role, entity.IsActive, entity.ProjectId);
}
