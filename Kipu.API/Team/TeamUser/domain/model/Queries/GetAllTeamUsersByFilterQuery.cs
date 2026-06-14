namespace Kipu.API.Team.TeamUser.domain.model.Queries;

public record GetAllTeamUsersByFilterQuery(string ProjectId, string? GlobalSearch, string? Role, bool? IsActive);