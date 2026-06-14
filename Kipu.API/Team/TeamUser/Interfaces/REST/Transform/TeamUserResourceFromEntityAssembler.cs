using Kipu.API.Team.TeamUser.Interfaces.REST.Resources;

namespace Kipu.API.Team.TeamUser.Interfaces.REST.Transform;

public static class TeamUserResourceFromEntityAssembler
{
    public static TeamUserResource ToResourceFromEntity(domain.model.Aggregates.TeamUser entity)
    {
        return new TeamUserResource(
            entity.Id.Value, 
            entity.FullName,
            entity.Email.Address, 
            entity.Role,
            entity.IsActive,
            entity.ProjectId);
    }
}