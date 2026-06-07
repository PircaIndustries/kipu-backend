using Kipu.API.IAM.Application.Services;
using Kipu.API.IAM.Interfaces.REST.Resources;
using Kipu.API.IAM.Interfaces.REST.Transform;
using Microsoft.AspNetCore.Mvc;

namespace Kipu.API.IAM.Interfaces.REST;

[ApiController]
[Route("api/v1/[controller]")]
public class AuthController(IUserCommandService userCommandService) : ControllerBase
{
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] SignInResource resource)
    {
        var command = SignInCommandFromResourceAssembler.ToCommandFromResource(resource);
        var result = await userCommandService.Handle(command);

        return result.Fold<IActionResult>(
            success => Ok(AuthenticatedUserResourceFromEntityAssembler.ToResourceFromEntity(success.User, success.Token)),
            error => Unauthorized(new { message = error })
        );
    }
}
