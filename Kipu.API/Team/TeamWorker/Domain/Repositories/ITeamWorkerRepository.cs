using Kipu.API.Shared.Domain.Repositories;
using Kipu.API.Team.TeamWorker.Domain.Model.ValueObjects;

namespace Kipu.API.Team.TeamWorker.Domain.Repositories;

public interface ITeamWorkerRepository : IBaseAggregateRepository<Model.Aggregates.TeamWorker, WorkerId>
{
    Task<IEnumerable<Model.Aggregates.TeamWorker>> FindByProjectIdAsync(string projectId, string? globalSearch);
}