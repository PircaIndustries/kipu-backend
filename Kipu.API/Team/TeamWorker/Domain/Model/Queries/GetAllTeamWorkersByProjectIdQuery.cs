namespace Kipu.API.Team.TeamWorker.Domain.Model.Queries;

public record GetAllTeamWorkersByProjectIdQuery(string ProjectId, string? GlobalSearch = null);