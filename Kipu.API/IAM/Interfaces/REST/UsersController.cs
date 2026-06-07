using Kipu.API.IAM.Application.Services;
using Kipu.API.IAM.Domain.Model.Queries;
using Kipu.API.IAM.Interfaces.REST.Resources;
using Kipu.API.IAM.Interfaces.REST.Transform;
using Microsoft.AspNetCore.Mvc;

namespace Kipu.API.IAM.Interfaces.REST;

[ApiController]
[Route("api/v1/[controller]")]
public class UsersController(
    IUserCommandService userCommandService,
    IUserQueryService userQueryService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateUser([FromBody] SignUpResource resource)
    {
        var signUpCommand = SignUpCommandFromResourceAssembler.ToCommandFromResource(resource);
        var result = await userCommandService.Handle(signUpCommand);

        return result.Fold<IActionResult>(
            user => CreatedAtAction(nameof(GetUserById), new { id = user.Id }, UserResourceFromEntityAssembler.ToResourceFromEntity(user)),
            error => BadRequest(new { message = error })
        );
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetUserById(int id)
    {
        var query = new GetUserByIdQuery(id);
        var user = await userQueryService.Handle(query);

        if (user == null)
        {
            return NotFound(new { message = $"User with ID {id} not found." });
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
            error => BadRequest(new { message = error })
        );
    }
}
