using Kipu.API.Team.TeamUser.domain.model.Queries;

namespace Kipu.API.Team.TeamUser.application.Services;

public interface ITeamUserQueryService
{
    Task<domain.model.Aggregates.TeamUser?> Handle(GetTeamUserByIdQuery query);
    Task<IEnumerable<domain.model.Aggregates.TeamUser>> Handle(GetAllTeamUsersByRoleQuery query);
    Task<IEnumerable<domain.model.Aggregates.TeamUser>> Handle(GetAllTeamUsersQuery query);
    Task<IEnumerable<domain.model.Aggregates.TeamUser>> Handle(GetAllTeamUsersByIsActiveQuery query);
    Task<IEnumerable<domain.model.Aggregates.TeamUser>> Handle(GetAllTeamUsersByFilterQuery query);
}