using Kipu.API.Team.TeamWorker.Domain.Model.Queries;

namespace Kipu.API.Team.TeamWorker.Application.Services;

public interface ITeamWorkerQueryService
{
    Task<Domain.Model.Aggregates.TeamWorker?> Handle(GetTeamWorkerByIdQuery query);
    Task<IEnumerable<Domain.Model.Aggregates.TeamWorker>> Handle(GetAllTeamWorkersByProjectIdQuery query);
}