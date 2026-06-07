using Kipu.API.IAM.Domain.Model.Commands;
using Kipu.API.IAM.Interfaces.REST.Resources;

namespace Kipu.API.IAM.Interfaces.REST.Transform;

public static class UpdateUserRolesCommandFromResourceAssembler
{
    public static UpdateUserRolesCommand ToCommandFromResource(int id, UpdateUserRolesResource resource)
    {
        return new UpdateUserRolesCommand(id, resource.Roles);
    }
}
