using Kipu.API.Projects.Domain.Model.Aggregates;
using Kipu.API.Projects.Domain.Model.Queries;

namespace Kipu.API.Projects.Application.Services;

public interface IProjectQueryService
{
    Task<IEnumerable<Project>> Handle(GetAllProjectsQuery query);
    Task<Project?> Handle(GetProjectByIdQuery query);
    Task<IEnumerable<ProjectItem>> Handle(GetProjectItemsQuery query);
}
