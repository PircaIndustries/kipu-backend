using System.Net.Mime;
using Kipu.API.Logistics.MachineryCatalog.Application.Services;
using Kipu.API.Logistics.MachineryCatalog.Domain.Model.Aggregates;
using Kipu.API.Logistics.MachineryCatalog.Domain.Model.Queries;
using Kipu.API.Logistics.MachineryCatalog.Interfaces.REST.Resources;
using Kipu.API.Logistics.MachineryCatalog.Interfaces.REST.Transform;
using Kipu.API.Resources;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Swashbuckle.AspNetCore.Annotations;

namespace Kipu.API.Logistics.MachineryCatalog.Interfaces.REST;

[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[Tags("Machinery Catalog")]
public class MachineryCatalogController(
    IMachineryCatalogCommandService commandService,
    IMachineryCatalogQueryService queryService,
    IStringLocalizer<SharedResource> localizer,
    ILogger<MachineryCatalogController> logger) : ControllerBase
{
    [HttpPost]
    [Authorize(Roles = "Logística y Administración, Administrador")]
    [SwaggerOperation(Summary = "Creates a machinery catalog entry")]
    [SwaggerResponse(201, "The machinery catalog was created", typeof(MachineryCatalogResource))]
    [SwaggerResponse(400, "The request payload is invalid")]
    public async Task<ActionResult> CreateMachineryCatalog(
        [FromBody] CreateMachineryCatalogResource resource,
        CancellationToken cancellationToken)
    {
        try
        {
            var command = CreateMachineryCatalogCommandFromResourceAssembler.ToCommandFromResource(resource);
            var result = await commandService.Handle(command, cancellationToken);
            if (result is null)
                return BadRequest(localizer["UnexpectedErrorCreatingMachineryCatalog"].Value);
            return CreatedAtAction(nameof(GetMachineryCatalogById), new { id = result.Id },
                MachineryCatalogResourceFromEntityAssembler.ToResourceFromEntity(result));
        }
        catch (ArgumentException ex)
        {
            logger.LogWarning(ex, "Validation failed while creating machinery catalog");
            return BadRequest(localizer["InvalidMachineryCatalogRequest"].Value);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error creating machinery catalog");
            return Problem(title: localizer["UnexpectedServerError"].Value,
                detail: localizer["UnexpectedErrorCreatingMachineryCatalog"].Value, statusCode: 500);
        }
    }

    [HttpGet("{id}")]
    [SwaggerOperation(Summary = "Gets a machinery catalog by ID")]
    [SwaggerResponse(200, "The machinery catalog was found", typeof(MachineryCatalogResource))]
    [SwaggerResponse(404, "The machinery catalog was not found")]
    public async Task<ActionResult> GetMachineryCatalogById(string id, CancellationToken cancellationToken)
    {
        var query = new GetMachineryCatalogByIdQuery(id);
        var result = await queryService.Handle(query, cancellationToken);
        if (result is null)
            return NotFound(localizer["MachineryCatalogNotFound"].Value);
        return Ok(MachineryCatalogResourceFromEntityAssembler.ToResourceFromEntity(result));
    }

    [HttpGet]
    [SwaggerOperation(Summary = "Gets all machinery catalog entries")]
    [SwaggerResponse(200, "The machinery catalog entries were found", typeof(IEnumerable<MachineryCatalogResource>))]
    public async Task<ActionResult> GetAllMachineryCatalogs(CancellationToken cancellationToken)
    {
        var query = new GetAllMachineryCatalogsQuery();
        var results = await queryService.Handle(query, cancellationToken);
        var resources = results.Select(MachineryCatalogResourceFromEntityAssembler.ToResourceFromEntity);
        return Ok(resources);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Logística y Administración, Administrador")]
    [SwaggerOperation(Summary = "Deletes a machinery catalog entry")]
    [SwaggerResponse(204, "The machinery catalog was deleted")]
    [SwaggerResponse(404, "The machinery catalog was not found")]
    public async Task<ActionResult> DeleteMachineryCatalog(string id, CancellationToken cancellationToken)
    {
        try
        {
            var query = new GetMachineryCatalogByIdQuery(id);
            var existing = await queryService.Handle(query, cancellationToken);
            if (existing is null)
                return NotFound(localizer["MachineryCatalogNotFound"].Value);
            var repository = HttpContext.RequestServices
                .GetRequiredService<Logistics.MachineryCatalog.Domain.Repositories.IMachineryCatalogRepository>();
            repository.Remove(existing);
            var unitOfWork = HttpContext.RequestServices
                .GetRequiredService<Shared.Domain.Repositories.IUnitOfWork>();
            await unitOfWork.CompleteAsync(cancellationToken);
            return NoContent();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error deleting machinery catalog {Id}", id);
            return Problem(title: localizer["UnexpectedServerError"].Value,
                detail: localizer["UnexpectedErrorDeletingMachineryCatalog"].Value, statusCode: 500);
        }
    }

    [HttpPatch("{id}")]
    [Authorize(Roles = "Logística y Administración, Administrador")]
    [SwaggerOperation(Summary = "Partially updates a machinery catalog entry")]
    [SwaggerResponse(200, "The machinery catalog was updated", typeof(MachineryCatalogResource))]
    [SwaggerResponse(400, "The request payload is invalid")]
    [SwaggerResponse(404, "The machinery catalog was not found")]
    public async Task<ActionResult> PatchMachineryCatalog(string id,
        [FromBody] UpdatePartialMachineryCatalogResource resource,
        CancellationToken cancellationToken)
    {
        try
        {
            var command = UpdatePartialMachineryCatalogCommandFromResourceAssembler.ToCommandFromResource(id, resource);
            var result = await commandService.Handle(command, cancellationToken);
            if (result is null)
                return NotFound(localizer["MachineryCatalogNotFound"].Value);
            return Ok(MachineryCatalogResourceFromEntityAssembler.ToResourceFromEntity(result));
        }
        catch (ArgumentException ex)
        {
            logger.LogWarning(ex, "Validation failed while partially updating machinery catalog {Id}", id);
            return BadRequest(localizer["InvalidMachineryCatalogRequest"].Value);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error partially updating machinery catalog {Id}", id);
            return Problem(title: localizer["UnexpectedServerError"].Value,
                detail: localizer["UnexpectedErrorUpdatingMachineryCatalog"].Value, statusCode: 500);
        }
    }
}
