using System.Net.Mime;
using Kipu.API.Logistics.Application.Services;
using Kipu.API.Logistics.Domain.Model.Queries;
using Kipu.API.Logistics.Interfaces.REST.Resources;
using Kipu.API.Logistics.Interfaces.REST.Transform.MaterialCatalog;
using Kipu.API.Resources;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Swashbuckle.AspNetCore.Annotations;
namespace Kipu.API.Logistics.Interfaces.REST.Transform;

[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[Tags("Material Catalog")]
public class MaterialCatalogController(
    IMaterialCatalogCommandService materialCatalogCommandService,
    IMaterialCatalogQueryService materialCatalogQueryService,
    IStringLocalizer<SharedResource> localizer,
    ILogger<MaterialCatalogController> logger) : ControllerBase
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
}