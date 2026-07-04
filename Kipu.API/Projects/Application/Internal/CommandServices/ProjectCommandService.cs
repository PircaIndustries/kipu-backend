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
            return new Result<Project, string>.Failure("ProjectNameAlreadyExists");
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
            return new Result<Project, string>.Failure(ex.Message);
        }
    }

    public async Task<Result<Project, string>> Handle(UpdateProjectStatusCommand command)
    {
        var project = await projectRepository.FindByIdAsync(command.ProjectId);
        if (project == null)
        {
            return new Result<Project, string>.Failure("ProjectNotFound");
        }

        if (string.IsNullOrWhiteSpace(command.Justification))
        {
            return new Result<Project, string>.Failure("JustificationRequired");
        }

        if (!ProjectStatus.IsValid(command.Status))
        {
            return new Result<Project, string>.Failure("InvalidProjectStatus");
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
            return new Result<Project, string>.Failure(ex.Message);
        }
    }

    public async Task<Result<IEnumerable<ProjectItem>, string>> Handle(AddProjectItemsCommand command)
    {
        var project = await projectRepository.FindByIdAsync(command.ProjectId);
        if (project == null)
        {
            return new Result<IEnumerable<ProjectItem>, string>.Failure("ProjectNotFound");
        }

        // Validate items dates
        foreach (var item in command.Items)
        {
            if (item.EndDate < item.StartDate)
            {
                return new Result<IEnumerable<ProjectItem>, string>.Failure("InvalidItemDates");
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
            return new Result<IEnumerable<ProjectItem>, string>.Failure(ex.Message);
        }
    }
}
