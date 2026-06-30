using Kipu.API.IAM.Domain.Model.Commands;
using Kipu.API.IAM.Interfaces.REST.Resources;

namespace Kipu.API.IAM.Interfaces.REST.Transform;

public static class ResetPasswordCommandFromResourceAssembler
{
    public static ResetPasswordCommand ToCommandFromResource(ResetPasswordResource resource)
    {
        return new ResetPasswordCommand(resource.Email, resource.Code, resource.NewPassword);
    }
}
