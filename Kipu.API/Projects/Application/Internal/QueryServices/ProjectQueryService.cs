using Kipu.API.Projects.Application.Services;
using Kipu.API.Projects.Domain.Model.Aggregates;
using Kipu.API.Projects.Domain.Model.Queries;
using Kipu.API.Projects.Domain.Repositories;
using Kipu.API.Team.TeamUser.domain.Repositories;
using System.Linq;

namespace Kipu.API.Projects.Application.Internal.QueryServices;

public class ProjectQueryService(
    IProjectRepository projectRepository,
    IProjectItemRepository projectItemRepository,
    ITeamUserRepository teamUserRepository) : IProjectQueryService
{
    public async Task<IEnumerable<Project>> Handle(GetAllProjectsQuery query)
    {
        if (string.IsNullOrEmpty(query.UserId)) return new List<Project>();

        // Find projects where the user is a team member
        if (!int.TryParse(query.UserId, out int parsedUserId)) return new List<Project>();
        var teamUsers = await teamUserRepository.FindByUserIdAsync(parsedUserId);
        var projectIds = teamUsers.Select(tu => tu.ProjectId).ToList();
        
        var allProjects = await projectRepository.ListAsync();
        return allProjects.Where(p => projectIds.Contains(p.Id.ToString()));
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
