using Kipu.API.Progress.Domain.Model.Aggregates;
using Kipu.API.Shared.Domain.Repositories;

namespace Kipu.API.Progress.Domain.Repositories;

public interface IProgressItemRepository : IBaseRepository<ProgressItem>
{
    Task<IEnumerable<ProgressItem>> FindByProjectIdAsync(int projectId);
}