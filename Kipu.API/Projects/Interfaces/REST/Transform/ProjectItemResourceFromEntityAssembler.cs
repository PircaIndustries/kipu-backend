using Kipu.API.Projects.Domain.Model.Aggregates;
using Kipu.API.Projects.Interfaces.REST.Resources;

namespace Kipu.API.Projects.Interfaces.REST.Transform;

public static class ProjectItemResourceFromEntityAssembler
{
    public static ProjectItemResource ToResourceFromEntity(ProjectItem entity)
    {
        return new ProjectItemResource(
            entity.Id,
            entity.Name,
            entity.StartDate,
            entity.EndDate,
            entity.ProjectId
        );
    }
}
