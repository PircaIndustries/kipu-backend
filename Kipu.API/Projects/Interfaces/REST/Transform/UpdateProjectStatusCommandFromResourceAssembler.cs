using Kipu.API.Projects.Domain.Model.Commands;
using Kipu.API.Projects.Interfaces.REST.Resources;

namespace Kipu.API.Projects.Interfaces.REST.Transform;

public static class UpdateProjectStatusCommandFromResourceAssembler
{
    public static UpdateProjectStatusCommand ToCommandFromResource(int projectId, UpdateProjectStatusResource resource)
    {
        return new UpdateProjectStatusCommand(projectId, resource.Status, resource.StatusJustification);
    }
}
