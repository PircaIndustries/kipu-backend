using System.Net.Mime;
using Kipu.API.Logistics.Application.Errors;
using Kipu.API.Logistics.Application.Services;
using Kipu.API.Logistics.Domain.Model.Aggregates;
using Kipu.API.Logistics.Domain.Model.Commands;
using Kipu.API.Logistics.Domain.Model.Queries;
using Kipu.API.Logistics.Interfaces.REST.Resources.MachineryAssignment;
using Kipu.API.Logistics.Interfaces.REST.Transform.MachineryAssignment;
using Kipu.API.Resources;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Kipu.API.Logistics.Interfaces.REST;

[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[Tags("Machinery Assignments")]
public class MachineryAssignmentsController(
    IMachineryAssignmentCommandService commandService,
    IMachineryAssignmentQueryService queryService,
    IStringLocalizer<SharedResource> localizer,
    ILogger<MachineryAssignmentsController> logger) : ControllerBase
{
    [HttpPost]
    [SwaggerOperation(Summary = "Creates a machinery assignment")]
    [SwaggerResponse(201, "The machinery assignment was created", typeof(MachineryAssignmentResource))]
    [SwaggerResponse(400, "The request payload is invalid")]
    [SwaggerResponse(500, "Unexpected server error")]
    public async Task<ActionResult> CreateMachineryAssignment(
        [FromBody] CreateMachineryAssignmentResource resource,
        CancellationToken cancellationToken)
    {
        try
        {
            var command = CreateMachineryAssignmentCommandFromResourceAssembler.ToCommandFromResource(resource);
            var result = await commandService.Handle(command, cancellationToken);
            return result switch
            {
                Result<MachineryAssignment, CreateMachineryAssignmentError>.Success success => CreatedAtAction(
                    nameof(GetMachineryAssignmentById), new { id = success.Value.Id },
                    MachineryAssignmentResourceFromEntityAssembler.ToResourceFromEntity(success.Value)),
                _ => BadRequest(localizer["UnexpectedErrorCreatingMachineryAssignment"].Value)
            };
        }
        catch (ArgumentException ex)
        {
            logger.LogWarning(ex, "Validation failed while creating machinery assignment");
            return BadRequest(localizer["InvalidMachineryAssignmentRequest"].Value);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error while creating machinery assignment");
            return Problem(title: localizer["UnexpectedServerError"].Value,
                detail: localizer["UnexpectedErrorCreatingMachineryAssignment"].Value, statusCode: 500);
        }
    }

    [HttpGet("{id}")]
    [SwaggerOperation(Summary = "Gets a machinery assignment by ID")]
    [SwaggerResponse(200, "The machinery assignment was found", typeof(MachineryAssignmentResource))]
    [SwaggerResponse(404, "The machinery assignment was not found")]
    public async Task<ActionResult> GetMachineryAssignmentById(string id, CancellationToken cancellationToken)
    {
        var query = new GetMachineryAssignmentByIdQuery(id);
        var result = await queryService.Handle(query, cancellationToken);
        if (result is null)
            return NotFound(localizer["MachineryAssignmentNotFound"].Value);
        return Ok(MachineryAssignmentResourceFromEntityAssembler.ToResourceFromEntity(result));
    }

    [HttpGet]
    [SwaggerOperation(Summary = "Gets machinery assignments by project ID")]
    [SwaggerResponse(200, "The machinery assignments were found", typeof(IEnumerable<MachineryAssignmentResource>))]
    public async Task<ActionResult> GetMachineryAssignmentsByProject(
        [FromQuery] string projectId,
        CancellationToken cancellationToken)
    {
        var query = new GetMachineryAssignmentsByProjectIdQuery(projectId);
        var results = await queryService.Handle(query, cancellationToken);
        var resources = results.Select(MachineryAssignmentResourceFromEntityAssembler.ToResourceFromEntity);
        return Ok(resources);
    }

    [HttpPatch("{id}")]
    [SwaggerOperation(Summary = "Partially updates a machinery assignment")]
    [SwaggerResponse(200, "The machinery assignment was updated", typeof(MachineryAssignmentResource))]
    [SwaggerResponse(400, "The request payload is invalid")]
    [SwaggerResponse(404, "The machinery assignment was not found")]
    public async Task<ActionResult> UpdateMachineryAssignment(
        string id, [FromBody] UpdateMachineryAssignmentResource resource,
        CancellationToken cancellationToken)
    {
        try
        {
            var command = UpdateMachineryAssignmentCommandFromResourceAssembler.ToCommandFromResource(id, resource);
            var result = await commandService.Handle(command, cancellationToken);
            return result switch
            {
                Result<MachineryAssignment, UpdateMachineryAssignmentError>.Success success => Ok(
                    MachineryAssignmentResourceFromEntityAssembler.ToResourceFromEntity(success.Value)),
                Result<MachineryAssignment, UpdateMachineryAssignmentError>.Failure { Error: UpdateMachineryAssignmentError.NotFound }
                    => NotFound(localizer["MachineryAssignmentNotFound"].Value),
                _ => BadRequest(localizer["UnexpectedErrorUpdatingMachineryAssignment"].Value)
            };
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error updating machinery assignment {Id}", id);
            return Problem(title: localizer["UnexpectedServerError"].Value,
                detail: localizer["UnexpectedErrorUpdatingMachineryAssignment"].Value, statusCode: 500);
        }
    }

    [HttpDelete("{id}")]
    [SwaggerOperation(Summary = "Deletes a machinery assignment")]
    [SwaggerResponse(204, "The machinery assignment was deleted")]
    [SwaggerResponse(404, "The machinery assignment was not found")]
    public async Task<ActionResult> DeleteMachineryAssignment(string id, CancellationToken cancellationToken)
    {
        try
        {
            var result = await commandService.HandleDelete(id, cancellationToken);
            return result switch
            {
                Result<MachineryAssignment, UpdateMachineryAssignmentError>.Success => NoContent(),
                _ => NotFound(localizer["MachineryAssignmentNotFound"].Value)
            };
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error deleting machinery assignment {Id}", id);
            return Problem(title: localizer["UnexpectedServerError"].Value,
                detail: localizer["UnexpectedErrorDeletingMachineryAssignment"].Value, statusCode: 500);
        }
    }
}
