using Kipu.API.IAM.Application.Services;
using Kipu.API.IAM.Interfaces.REST.Resources;
using Kipu.API.IAM.Interfaces.REST.Transform;
using Kipu.API.Resources;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

using Microsoft.AspNetCore.Authorization;

namespace Kipu.API.IAM.Interfaces.REST;

[ApiController]
[AllowAnonymous]
[Route("api/v1/[controller]")]
public class AuthController(
    IUserCommandService userCommandService,
    IStringLocalizer<SharedResource> localizer) : ControllerBase
{
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] SignInResource resource)
    {
        var command = SignInCommandFromResourceAssembler.ToCommandFromResource(resource);
        var result = await userCommandService.Handle(command);

        return result.Fold<IActionResult>(
            success => 
            {
                if (success.Token == "OTP_SENT")
                {
                    return Ok(new { message = "Se ha enviado un código de verificación a tu correo.", step = "VERIFICATION_REQUIRED" });
                }
                return Ok(AuthenticatedUserResourceFromEntityAssembler.ToResourceFromEntity(success.User, success.Token));
            },
            error => Unauthorized(new { message = localizer[error].Value })
        );
    }

    [HttpPost("verify-login")]
    public async Task<IActionResult> VerifyLogin([FromBody] VerifyLoginResource resource)
    {
        var command = VerifyLoginCommandFromResourceAssembler.ToCommandFromResource(resource);
        var result = await userCommandService.Handle(command);

        return result.Fold<IActionResult>(
            success => Ok(AuthenticatedUserResourceFromEntityAssembler.ToResourceFromEntity(success.User, success.Token)),
            error => Unauthorized(new { message = localizer[error].Value ?? error })
        );
    }

    [HttpPost("forgot-password")]
    public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordResource resource)
    {
        var command = ForgotPasswordCommandFromResourceAssembler.ToCommandFromResource(resource);
        var result = await userCommandService.Handle(command);

        return result.Fold<IActionResult>(
            success => Ok(new { message = "Si el correo está registrado, se ha enviado un código de recuperación." }),
            error => BadRequest(new { message = localizer[error].Value ?? error })
        );
    }

    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordResource resource)
    {
        var command = ResetPasswordCommandFromResourceAssembler.ToCommandFromResource(resource);
        var result = await userCommandService.Handle(command);

        return result.Fold<IActionResult>(
            success => Ok(new { message = "Contraseña cambiada exitosamente." }),
            error => BadRequest(new { message = localizer[error].Value ?? error })
        );
    }
}
