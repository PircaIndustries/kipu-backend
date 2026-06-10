using Kipu.API.Projects.Domain.Model.Aggregates;
using Kipu.API.Shared.Domain.Repositories;

namespace Kipu.API.Projects.Domain.Repositories;

public interface IProjectItemRepository : IBaseRepository<ProjectItem>
{
    Task<IEnumerable<ProjectItem>> FindByProjectIdAsync(int projectId);
}
