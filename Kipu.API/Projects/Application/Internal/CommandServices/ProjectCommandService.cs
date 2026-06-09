using Kipu.API.Projects.Application.Services;
using Kipu.API.Projects.Domain.Model.Aggregates;
using Kipu.API.Projects.Domain.Model.Commands;
using Kipu.API.Projects.Domain.Model.ValueObjects;
using Kipu.API.Projects.Domain.Repositories;
using Kipu.API.Shared.Application.Patterns;
using Kipu.API.Shared.Domain.Repositories;

namespace Kipu.API.Projects.Application.Internal.CommandServices;

public class ProjectCommandService(
    IProjectRepository projectRepository,
    IProjectItemRepository projectItemRepository,
    IUnitOfWork unitOfWork) : IProjectCommandService
{
    public async Task<Result<Project, string>> Handle(CreateProjectCommand command)
    {
        if (await projectRepository.ExistsByNameAsync(command.Name))
        {
            return new Result<Project, string>.Failure($"A project with name '{command.Name}' already exists.");
        }

        var project = new Project(
            command.Name,
            command.Location,
            command.Budget,
            command.Description,
            command.StartDate,
            command.EndDate,
            command.Image,
            command.Members,
            command.Rnc,
            command.Pending,
            command.Status,
            command.StatusJustification
        );

        try
        {
            await projectRepository.AddAsync(project);
            await unitOfWork.CompleteAsync();
            return new Result<Project, string>.Success(project);
        }
        catch (Exception ex)
        {
            return new Result<Project, string>.Failure($"An error occurred while creating the project: {ex.Message}");
        }
    }

    public async Task<Result<Project, string>> Handle(UpdateProjectStatusCommand command)
    {
        var project = await projectRepository.FindByIdAsync(command.ProjectId);
        if (project == null)
        {
            return new Result<Project, string>.Failure($"Project with ID {command.ProjectId} not found.");
        }

        if (string.IsNullOrWhiteSpace(command.Justification))
        {
            return new Result<Project, string>.Failure("A justification is required for changing the project status.");
        }

        if (!ProjectStatus.IsValid(command.Status))
        {
            return new Result<Project, string>.Failure($"Invalid status '{command.Status}'. Valid statuses are: {string.Join(", ", ProjectStatus.All)}.");
        }

        project.Status = command.Status;
        project.StatusJustification = command.Justification;

        try
        {
            projectRepository.Update(project);
            await unitOfWork.CompleteAsync();
            return new Result<Project, string>.Success(project);
        }
        catch (Exception ex)
        {
            return new Result<Project, string>.Failure($"An error occurred while updating the project status: {ex.Message}");
        }
    }

    public async Task<Result<IEnumerable<ProjectItem>, string>> Handle(AddProjectItemsCommand command)
    {
        var project = await projectRepository.FindByIdAsync(command.ProjectId);
        if (project == null)
        {
            return new Result<IEnumerable<ProjectItem>, string>.Failure($"Project with ID {command.ProjectId} not found.");
        }

        // Validate items dates
        foreach (var item in command.Items)
        {
            if (item.EndDate < item.StartDate)
            {
                return new Result<IEnumerable<ProjectItem>, string>.Failure($"For item '{item.Name}', the end date ({item.EndDate:yyyy-MM-dd}) cannot be earlier than the start date ({item.StartDate:yyyy-MM-dd}).");
            }
        }

        var projectItems = new List<ProjectItem>();
        foreach (var item in command.Items)
        {
            var projectItem = new ProjectItem(item.Name, item.StartDate, item.EndDate, command.ProjectId);
            await projectItemRepository.AddAsync(projectItem);
            projectItems.Add(projectItem);
        }

        try
        {
            await unitOfWork.CompleteAsync();
            return new Result<IEnumerable<ProjectItem>, string>.Success(projectItems);
        }
        catch (Exception ex)
        {
            return new Result<IEnumerable<ProjectItem>, string>.Failure($"An error occurred while adding project items: {ex.Message}");
        }
    }
}
