using System.Net.Mime;
using Kipu.API.Logistics.Application.Errors;
using Kipu.API.Logistics.Application.Services;
using Kipu.API.Logistics.Domain.Model.Aggregates;
using Kipu.API.Logistics.Domain.Model.Queries;
using Kipu.API.Logistics.Interfaces.REST.Resources;
using Kipu.API.Logistics.Interfaces.REST.Transform;
using Kipu.API.Logistics.Interfaces.REST.Transform.MaterialCatalog;
using Kipu.API.Resources;
using Kipu.API.Shared.Application.Patterns;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Swashbuckle.AspNetCore.Annotations;
namespace Kipu.API.Logistics.Interfaces.REST;

[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[Tags("Material Catalog")]
public class MaterialsCatalogController(
    IMaterialCatalogCommandService materialCatalogCommandService,
    IMaterialCatalogQueryService materialCatalogQueryService,
    IStringLocalizer<SharedResource> localizer,
    ILogger<MaterialsCatalogController> logger) : ControllerBase
{
    /// <summary>
    /// Creates a new material catalog entry
    /// </summary>
    /// <param name="resource">The material catalog data (Name, CategoryId, MeasureUnit)</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The created material catalog</returns>
    [HttpPost]
    [SwaggerOperation(
        Summary = "Creates a Material Catalog",
        Description = "Creates a Material Catalog with a given Name, CategoryId and MeasureUnit",
        OperationId = "CreateMaterialCatalog")]
    [SwaggerResponse(201, "The material catalog was created", typeof(MaterialCatalogResource))]
    [SwaggerResponse(400, "The request payload is invalid", typeof(string))]
    [SwaggerResponse(409, "The material catalog already exists", typeof(string))]
    [SwaggerResponse(500, "Unexpected server error", typeof(ProblemDetails))]
    public async Task<ActionResult> CreateMaterialCatalog([FromBody] CreateMaterialCatalogResource resource,
        CancellationToken cancellationToken)
    {
        try
        {
            var createMaterialCatalogCommand =
                CreateMaterialCatalogCommandFromResourceAssembler.ToCommandFromResource(resource);
            var result = await materialCatalogCommandService.Handle(createMaterialCatalogCommand, cancellationToken);
            return ActionResultFromCreateMaterialCatalogAssembler.ToActionResultFromCreateMaterialCatalogResult(
                result,
                this,
                localizer,
                nameof(GetMaterialCatalogById));
        }
        catch (ArgumentException ex)
        {
            logger.LogWarning(ex,
                "Validation failed while creating material catalog for Name {Name}, CategoryId {CategoryId}, MeasureUnit {MeasureUnit}",
                resource.Name, resource.CategoryId, resource.MeasureUnit);
            return BadRequest(localizer["InvalidMaterialCatalogRequest"].Value);
        }
        catch (Exception ex)
        {
            logger.LogError(ex,
                "Unexpected error while creating material catalog for Name {Name}, CategoryId {CategoryId}, MeasureUnit {MeasureUnit}",
                resource.Name, resource.CategoryId, resource.MeasureUnit);

            return Problem(
                title: localizer["UnexpectedServerError"].Value,
                detail: localizer["UnexpectedErrorCreatingMaterialCatalog"].Value,
                statusCode: 500);
        }
    }

    /// <summary>
    /// Gets a material catalog by ID
    /// </summary>
    /// <param name="id">The material catalog identifier</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The material catalog if found</returns>
    [HttpGet("{id}")]
    [SwaggerOperation(
        Summary = "Gets a Material Catalog by id",
        Description = "Gets a material catalog for a given material catalog identifier",
        OperationId = "GetMaterialCatalogById")]
    [SwaggerResponse(200, "The material catalog was found", typeof(MaterialCatalogResource))]
    [SwaggerResponse(404, "The material catalog was not found")]
    public async Task<ActionResult> GetMaterialCatalogById(int id, CancellationToken cancellationToken = default)
    {
        var getMaterialCatalogByIdQuery = new GetMaterialCatalogByIdQuery(id);
        var result = await materialCatalogQueryService.Handle(getMaterialCatalogByIdQuery, cancellationToken);
        if (result is null) return NotFound(localizer["MaterialCatalogNotFound"].Value);
        var resource = MaterialCatalogResourceFromEntityAssembler.ToResourceFromEntity(result);
        return Ok(resource);
    }
    
    [HttpGet]
    [SwaggerOperation(
        Summary = "Gets all Materials Catalog",
        Description = "Gets all Materials catalog",
        OperationId = "GetAllMaterialsCatalog")]
    [SwaggerResponse(200, "The Materials catalog were found", typeof(IEnumerable<MaterialCatalogResource>))]
    public async Task<ActionResult> GetAllMaterialsCatalog(CancellationToken cancellationToken = default)
    {
        var query = new GetAllMaterialsCatalogQuery();
        var result = await materialCatalogQueryService.Handle(query, cancellationToken);
        var resources = result.Select(MaterialCatalogResourceFromEntityAssembler.ToResourceFromEntity);
        return Ok(resources);
    }

    /// <summary>
    /// Updates a material catalog entry
    /// </summary>
    /// <param name="id">The material catalog identifier</param>
    /// <param name="resource">The updated material catalog data</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The updated material catalog</returns>
    [HttpPut("{id}")]
    [SwaggerOperation(
        Summary = "Updates a Material Catalog",
        Description = "Updates an existing material catalog with the given ID",
        OperationId = "UpdateMaterialCatalog")]
    [SwaggerResponse(200, "The material catalog was updated", typeof(MaterialCatalogResource))]
    [SwaggerResponse(400, "The request payload is invalid", typeof(string))]
    [SwaggerResponse(404, "The material catalog was not found", typeof(string))]
    [SwaggerResponse(500, "Unexpected server error", typeof(ProblemDetails))]
    public async Task<ActionResult> UpdateMaterialCatalog(int id,
        [FromBody] UpdateMaterialCatalogResource resource,
        CancellationToken cancellationToken)
    {
        try
        {
            var command = UpdateMaterialCatalogCommandFromResourceAssembler.ToCommandFromResource(id, resource);
            var result = await materialCatalogCommandService.Handle(command, cancellationToken);
            return ActionResultFromUpdateMaterialCatalogAssembler.ToActionResultFromUpdateMaterialCatalogResult(
                result, this, localizer);
        }
        catch (ArgumentException ex)
        {
            logger.LogWarning(ex, "Validation failed while updating material catalog with id {Id}", id);
            return BadRequest(localizer["InvalidMaterialCatalogRequest"].Value);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error while updating material catalog with id {Id}", id);
            return Problem(
                title: localizer["UnexpectedServerError"].Value,
                detail: localizer["UnexpectedErrorUpdatingMaterialCatalog"].Value,
                statusCode: 500);
        }
    }

    /// <summary>
    /// Deletes a material catalog entry
    /// </summary>
    /// <param name="id">The material catalog identifier</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>No content if successful</returns>
    [HttpDelete("{id}")]
    [SwaggerOperation(
        Summary = "Deletes a Material Catalog",
        Description = "Deletes an existing material catalog with the given ID",
        OperationId = "DeleteMaterialCatalog")]
    [SwaggerResponse(204, "The material catalog was deleted")]
    [SwaggerResponse(404, "The material catalog was not found", typeof(string))]
    [SwaggerResponse(500, "Unexpected server error", typeof(ProblemDetails))]
    public async Task<ActionResult> DeleteMaterialCatalog(int id,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await materialCatalogCommandService.HandleDelete(id, cancellationToken);
            if (result is Result<MaterialCatalog, UpdateMaterialCatalogError>.Success)
                return NoContent();
            var failure = (Result<MaterialCatalog, UpdateMaterialCatalogError>.Failure)result;
            return failure.Error switch
            {
                UpdateMaterialCatalogError.MaterialCatalogNotFound =>
                    NotFound(localizer["MaterialCatalogNotFound"].Value),
                _ => Problem(
                    title: localizer["UnexpectedServerError"].Value,
                    detail: localizer["UnexpectedErrorDeletingMaterialCatalog"].Value,
                    statusCode: 500)
            };
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error while deleting material catalog with id {Id}", id);
            return Problem(
                title: localizer["UnexpectedServerError"].Value,
                detail: localizer["UnexpectedErrorDeletingMaterialCatalog"].Value,
                statusCode: 500);
        }
    }
}