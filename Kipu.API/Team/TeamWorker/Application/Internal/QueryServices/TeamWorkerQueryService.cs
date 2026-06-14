using Kipu.API.Team.TeamWorker.Application.Services;
using Kipu.API.Team.TeamWorker.Domain.Model.Queries;
using Kipu.API.Team.TeamWorker.Domain.Model.ValueObjects;
using Kipu.API.Team.TeamWorker.Domain.Repositories;

namespace Kipu.API.Team.TeamWorker.Application.Internal.QueryServices;

public class TeamWorkerQueryService : ITeamWorkerQueryService
{
    private readonly ITeamWorkerRepository _teamWorkerRepository;

    public TeamWorkerQueryService(ITeamWorkerRepository teamWorkerRepository)
    {
        _teamWorkerRepository = teamWorkerRepository;
    }

    public async Task<Domain.Model.Aggregates.TeamWorker?> Handle(GetTeamWorkerByIdQuery query)
    {
        return await _teamWorkerRepository.FindByIdAsync(new WorkerId(query.TeamWorkerId));
    }

    public async Task<IEnumerable<Domain.Model.Aggregates.TeamWorker>> Handle(GetAllTeamWorkersByProjectIdQuery query)
    {
        return await _teamWorkerRepository.FindByProjectIdAsync(query.ProjectId, query.GlobalSearch);
    }
}