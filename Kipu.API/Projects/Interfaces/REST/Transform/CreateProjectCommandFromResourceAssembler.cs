using Kipu.API.Projects.Domain.Model.Commands;
using Kipu.API.Projects.Interfaces.REST.Resources;

namespace Kipu.API.Projects.Interfaces.REST.Transform;

public static class CreateProjectCommandFromResourceAssembler
{
    public static CreateProjectCommand ToCommandFromResource(CreateProjectResource resource)
    {
        return new CreateProjectCommand(
            resource.Name,
            resource.Location,
            resource.Budget,
            resource.Description,
            resource.StartDate,
            resource.EndDate,
            resource.Image,
            resource.Members,
            resource.Rnc,
            resource.Pending,
            resource.Status,
            resource.StatusJustification
        );
    }
}
