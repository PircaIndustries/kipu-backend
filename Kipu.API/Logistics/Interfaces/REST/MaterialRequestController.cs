using System.Net.Mime;
using Kipu.API.Logistics.Application.Services;
using Kipu.API.Logistics.Domain.Model.Queries;
using Kipu.API.Logistics.Domain.Model.ValueObjects;
using Kipu.API.Logistics.Interfaces.REST.Resources.MaterialRequest;
using Kipu.API.Logistics.Interfaces.REST.Transform.MaterialRequest;
using Kipu.API.Resources;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Swashbuckle.AspNetCore.Annotations;

namespace Kipu.API.Logistics.Interfaces.REST;

[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[Tags("Material Request")]
public class MaterialRequestController(
    IMaterialRequestCommandService materialRequestCommandService,
    IMaterialRequestQueryService materialRequestQueryService,
    IStringLocalizer<SharedResource> localizer,
    ILogger<MaterialRequestController> logger) : ControllerBase
{
    [HttpPost]
    [SwaggerOperation(
        Summary = "Creates a Material Request",
        Description = "Creates a material request with the given deadline, priority, delivery location, budget line, purpose and notes",
        OperationId = "CreateMaterialRequest")]
    [SwaggerResponse(201, "The material request was created", typeof(MaterialRequestResource))]
    [SwaggerResponse(400, "The request payload is invalid", typeof(string))]
    [SwaggerResponse(500, "Unexpected server error", typeof(ProblemDetails))]
    public async Task<ActionResult> CreateMaterialRequest(
        [FromBody] CreateMaterialRequestResource resource,
        CancellationToken cancellationToken)
    {
        try
        {
            var command = CreateMaterialRequestCommandFromResourceAssembler.ToCommandFromResource(resource);
            var result = await materialRequestCommandService.Handle(command, cancellationToken);
            return ActionResultFromCreateMaterialRequestAssembler.ToActionResultFromCreateMaterialRequestResult(
                result,
                this,
                localizer,
                nameof(GetMaterialRequestById));
        }
        catch (ArgumentException ex)
        {
            logger.LogWarning(ex, "Validation failed while creating material request");
            return BadRequest(localizer["InvalidMaterialRequestRequest"].Value);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error while creating material request");
            return Problem(
                title: localizer["UnexpectedServerError"].Value,
                detail: localizer["UnexpectedErrorProcessingRequest"].Value,
                statusCode: 500);
        }
    }

    [HttpGet("{id}")]
    [SwaggerOperation(
        Summary = "Gets a Material Request by id",
        Description = "Gets a material request for a given identifier",
        OperationId = "GetMaterialRequestById")]
    [SwaggerResponse(200, "The material request was found", typeof(MaterialRequestResource))]
    [SwaggerResponse(404, "The material request was not found")]
    public async Task<ActionResult> GetMaterialRequestById(int id, CancellationToken cancellationToken = default)
    {
        var query = new GetMaterialRequestByIdQuery(id);
        var result = await materialRequestQueryService.Handle(query, cancellationToken);
        if (result is null)
            return NotFound(localizer["MaterialRequestNotFound"].Value);
        var resource = MaterialRequestResourceFromEntityAssembler.ToResourceFromEntity(result);
        return Ok(resource);
    }

    [HttpGet]
    [SwaggerOperation(
        Summary = "Gets all Material Requests",
        Description = "Gets all material requests",
        OperationId = "GetAllMaterialRequests")]
    [SwaggerResponse(200, "The material requests were found", typeof(IEnumerable<MaterialRequestResource>))]
    public async Task<ActionResult> GetAllMaterialRequests(CancellationToken cancellationToken = default)
    {
        var query = new GetAllMaterialRequestQuery();
        var result = await materialRequestQueryService.Handle(query, cancellationToken);
        var resources = result.Select(MaterialRequestResourceFromEntityAssembler.ToResourceFromEntity);
        return Ok(resources);
    }

    [HttpGet("status/{status}")]
    [SwaggerOperation(
        Summary = "Gets Material Requests by status",
        Description = "Gets all material requests filtered by status (Pending, Accepted, Refused)",
        OperationId = "GetMaterialRequestsByStatus")]
    [SwaggerResponse(200, "The material requests were found", typeof(IEnumerable<MaterialRequestResource>))]
    [SwaggerResponse(400, "Invalid status value")]
    public async Task<ActionResult> GetMaterialRequestsByStatus(string status, CancellationToken cancellationToken = default)
    {
        if (!Enum.TryParse<RequestStatus>(status, true, out var requestStatus))
            return BadRequest(localizer["InvalidMaterialRequestStatus"].Value);

        var query = new GetAllMaterialRequestByRequestStatusQuery(requestStatus);
        var result = await materialRequestQueryService.Handle(query, cancellationToken);
        var resources = result.Select(MaterialRequestResourceFromEntityAssembler.ToResourceFromEntity);
        return Ok(resources);
    }

    [HttpPut("{id}")]
    [SwaggerOperation(
        Summary = "Updates a Material Request",
        Description = "Updates an existing material request with the given ID. Cannot update requests with Accepted or Refused status.",
        OperationId = "UpdateMaterialRequest")]
    [SwaggerResponse(200, "The material request was updated", typeof(MaterialRequestResource))]
    [SwaggerResponse(400, "The request payload is invalid", typeof(string))]
    [SwaggerResponse(404, "The material request was not found", typeof(string))]
    [SwaggerResponse(409, "The material request cannot be updated due to its status", typeof(string))]
    [SwaggerResponse(500, "Unexpected server error", typeof(ProblemDetails))]
    public async Task<ActionResult> UpdateMaterialRequest(int id,
        [FromBody] UpdateMaterialRequestResource resource,
        CancellationToken cancellationToken)
    {
        try
        {
            var command = UpdateMaterialRequestCommandFromResourceAssembler.ToCommandFromResource(id, resource);
            var result = await materialRequestCommandService.Handle(command, cancellationToken);
            return ActionResultFromUpdateMaterialRequestAssembler.ToActionResultFromUpdateMaterialRequestResult(
                result, this, localizer);
        }
        catch (ArgumentException ex)
        {
            logger.LogWarning(ex, "Validation failed while updating material request with id {Id}", id);
            return BadRequest(localizer["InvalidMaterialRequestRequest"].Value);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error while updating material request with id {Id}", id);
            return Problem(
                title: localizer["UnexpectedServerError"].Value,
                detail: localizer["UnexpectedErrorProcessingRequest"].Value,
                statusCode: 500);
        }
    }

    [HttpPatch("{id}")]
    [SwaggerOperation(
        Summary = "Partially updates a Material Request",
        Description = "Partially updates an existing material request with the given ID. Only provided fields are updated. Cannot update requests with Accepted or Refused status.",
        OperationId = "UpdatePartialMaterialRequest")]
    [SwaggerResponse(200, "The material request was updated", typeof(MaterialRequestResource))]
    [SwaggerResponse(400, "The request payload is invalid", typeof(string))]
    [SwaggerResponse(404, "The material request was not found", typeof(string))]
    [SwaggerResponse(409, "The material request cannot be updated due to its status", typeof(string))]
    [SwaggerResponse(500, "Unexpected server error", typeof(ProblemDetails))]
    public async Task<ActionResult> UpdatePartialMaterialRequest(int id,
        [FromBody] UpdatePartialMaterialRequestResource resource,
        CancellationToken cancellationToken)
    {
        try
        {
            var command = UpdatePartialMaterialRequestCommandFromResourceAssembler.ToCommandFromResource(id, resource);
            var result = await materialRequestCommandService.Handle(command, cancellationToken);
            return ActionResultFromUpdatePartialMaterialRequestAssembler.ToActionResultFromUpdatePartialMaterialRequestResult(
                result, this, localizer);
        }
        catch (ArgumentException ex)
        {
            logger.LogWarning(ex, "Validation failed while patching material request with id {Id}", id);
            return BadRequest(localizer["InvalidMaterialRequestRequest"].Value);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error while patching material request with id {Id}", id);
            return Problem(
                title: localizer["UnexpectedServerError"].Value,
                detail: localizer["UnexpectedErrorProcessingRequest"].Value,
                statusCode: 500);
        }
    }
}
