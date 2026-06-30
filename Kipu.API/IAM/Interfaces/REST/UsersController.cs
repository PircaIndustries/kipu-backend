using Kipu.API.IAM.Application.Services;
using Kipu.API.IAM.Domain.Model.Queries;
using Kipu.API.IAM.Interfaces.REST.Resources;
using Kipu.API.IAM.Interfaces.REST.Transform;
using Kipu.API.Resources;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

using Microsoft.AspNetCore.Authorization;

namespace Kipu.API.IAM.Interfaces.REST;

[ApiController]
[Route("api/v1/[controller]")]
public class UsersController(
    IUserCommandService userCommandService,
    IUserQueryService userQueryService,
    IStringLocalizer<SharedResource> localizer) : ControllerBase
{
    [HttpPost]
    [HttpPost("~/api/v1/identities")]
    [AllowAnonymous]
    public async Task<IActionResult> CreateUser([FromBody] SignUpResource resource)
    {
        var signUpCommand = SignUpCommandFromResourceAssembler.ToCommandFromResource(resource);
        var result = await userCommandService.Handle(signUpCommand);

        return result.Fold<IActionResult>(
            user => CreatedAtAction(nameof(GetUserById), new { id = user.Id }, UserResourceFromEntityAssembler.ToResourceFromEntity(user)),
            error => BadRequest(new { message = localizer[error].Value })
        );
    }

    [HttpGet]
    [HttpGet("~/api/v1/identities")]
    [AllowAnonymous]
    public async Task<IActionResult> GetUserByEmail([FromQuery] string email)
    {
        var query = new GetUserByEmailQuery(email);
        var user = await userQueryService.Handle(query);

        if (user == null)
        {
            return Ok(new List<object>());
        }

        var resource = UserResourceFromEntityAssembler.ToResourceFromEntity(user);
        return Ok(new List<object> { resource });
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetUserById(int id)
    {
        var query = new GetUserByIdQuery(id);
        var user = await userQueryService.Handle(query);

        if (user == null)
        {
            return NotFound(new { message = string.Format(localizer["UserNotFoundWithId"].Value, id) });
        }

        var resource = UserResourceFromEntityAssembler.ToResourceFromEntity(user);
        return Ok(resource);
    }

    [HttpPut("{id:int}/roles")]
    public async Task<IActionResult> UpdateUserRoles(int id, [FromBody] UpdateUserRolesResource resource)
    {
        var command = UpdateUserRolesCommandFromResourceAssembler.ToCommandFromResource(id, resource);
        var result = await userCommandService.Handle(command);

        return result.Fold<IActionResult>(
            user => Ok(UserResourceFromEntityAssembler.ToResourceFromEntity(user)),
            error => BadRequest(new { message = localizer[error].Value })
        );
    }
}
