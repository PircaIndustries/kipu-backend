using Kipu.API.IAM.Domain.Model.Commands;
using Kipu.API.IAM.Interfaces.REST.Resources;

namespace Kipu.API.IAM.Interfaces.REST.Transform;

public static class ForgotPasswordCommandFromResourceAssembler
{
    public static ForgotPasswordCommand ToCommandFromResource(ForgotPasswordResource resource)
    {
        return new ForgotPasswordCommand(resource.Email);
    }
}
