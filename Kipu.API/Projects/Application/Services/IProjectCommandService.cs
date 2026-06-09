using Kipu.API.Projects.Domain.Model.Aggregates;
using Kipu.API.Projects.Domain.Model.Commands;
using Kipu.API.Shared.Application.Patterns;

namespace Kipu.API.Projects.Application.Services;

public interface IProjectCommandService
{
    Task<Result<Project, string>> Handle(CreateProjectCommand command);
    Task<Result<Project, string>> Handle(UpdateProjectStatusCommand command);
    Task<Result<IEnumerable<ProjectItem>, string>> Handle(AddProjectItemsCommand command);
}
