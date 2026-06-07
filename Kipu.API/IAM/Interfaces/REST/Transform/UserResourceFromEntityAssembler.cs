using Kipu.API.IAM.Domain.Model.Aggregates;
using Kipu.API.IAM.Interfaces.REST.Resources;

namespace Kipu.API.IAM.Interfaces.REST.Transform;

public static class UserResourceFromEntityAssembler
{
    public static UserResource ToResourceFromEntity(User entity)
    {
        return new UserResource(entity.Id, entity.Name, entity.Email, entity.Role);
    }
}
