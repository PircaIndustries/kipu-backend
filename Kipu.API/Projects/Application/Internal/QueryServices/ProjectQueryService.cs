using Kipu.API.Projects.Application.Services;
using Kipu.API.Projects.Domain.Model.Aggregates;
using Kipu.API.Projects.Domain.Model.Queries;
using Kipu.API.Projects.Domain.Repositories;

namespace Kipu.API.Projects.Application.Internal.QueryServices;

public class ProjectQueryService(
    IProjectRepository projectRepository,
    IProjectItemRepository projectItemRepository) : IProjectQueryService
{
    public async Task<IEnumerable<Project>> Handle(GetAllProjectsQuery query)
    {
        return await projectRepository.ListAsync();
    }

    public async Task<Project?> Handle(GetProjectByIdQuery query)
    {
        return await projectRepository.FindByIdAsync(query.Id);
    }

    public async Task<IEnumerable<ProjectItem>> Handle(GetProjectItemsQuery query)
    {
        return await projectItemRepository.FindByProjectIdAsync(query.ProjectId);
    }
}
