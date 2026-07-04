using Kipu.API.IAM.Domain.Model.Commands;
using Kipu.API.IAM.Interfaces.REST.Resources;

namespace Kipu.API.IAM.Interfaces.REST.Transform;

public static class VerifyLoginCommandFromResourceAssembler
{
    public static VerifyLoginCommand ToCommandFromResource(VerifyLoginResource resource)
    {
        return new VerifyLoginCommand(resource.Email, resource.Code, resource.RememberMe);
    }
}
