using Kipu.API.Projects.Domain.Model.Aggregates;
using Kipu.API.Shared.Domain.Repositories;

namespace Kipu.API.Projects.Domain.Repositories;

public interface IProjectRepository : IBaseRepository<Project>
{
    Task<Project?> FindByNameAsync(string name);
    Task<bool> ExistsByNameAsync(string name);
}
