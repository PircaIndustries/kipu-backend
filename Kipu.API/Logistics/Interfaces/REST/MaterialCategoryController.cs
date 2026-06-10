using System.Net.Mime;
using Kipu.API.Logistics.Application.Services;
using Kipu.API.Logistics.Domain.Model.Queries;
using Kipu.API.Logistics.Domain.Model.Queries.MaterialCategory;
using Kipu.API.Logistics.Interfaces.REST.Resources.MaterialCategory;
using Kipu.API.Logistics.Interfaces.REST.Transform.MaterialCategory;
using Kipu.API.Resources;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Swashbuckle.AspNetCore.Annotations;

namespace Kipu.API.Logistics.Interfaces.REST;

[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[Tags("Material Category")]
public class MaterialCategoryController(
    IMaterialCategoryCommandService materialCategoryCommandService,
    IMaterialCategoryQueryService materialCategoryQueryService,
    IStringLocalizer<SharedResource> localizer,
    ILogger<MaterialCategoryController> logger) : ControllerBase
{
    /// <summary>
    /// Creates a new material category
    /// </summary>
    /// <param name="resource">The material category data (Name, Description, IsActive)</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The created material category</returns>
    [HttpPost]
    [SwaggerOperation(
        Summary = "Creates a Material Category",
        Description = "Creates a Material Category with a given Name, Description and IsActive status",
        OperationId = "CreateMaterialCategory")]
    [SwaggerResponse(201, "The material category was created", typeof(MaterialCategoryResource))]
    [SwaggerResponse(400, "The request payload is invalid", typeof(string))]
    [SwaggerResponse(409, "The material category already exists", typeof(string))]
    [SwaggerResponse(500, "Unexpected server error", typeof(ProblemDetails))]
    public async Task<ActionResult> CreateMaterialCategory([FromBody] CreateMaterialCategoryResource resource,
        CancellationToken cancellationToken)
    {
        try
        {
            var createMaterialCategoryCommand =
                CreateMaterialCategoryCommandFromResourceAssembler.ToCommandFromResource(resource);
            var result = await materialCategoryCommandService.Handle(createMaterialCategoryCommand, cancellationToken);
            return ActionResultFromCreateMaterialCategoryAssembler.ToActionResultFromCreateMaterialCategoryResult(
                result,
                this,
                localizer,
                nameof(GetMaterialCategoryById));
        }
        catch (ArgumentException ex)
        {
            logger.LogWarning(ex,
                "Validation failed while creating material category for Name {Name}, Description {Description}, IsActive {IsActive}",
                resource.Name, resource.Description, resource.IsActive);
            return BadRequest(localizer["InvalidMaterialCategoryRequest"].Value);
        }
        catch (Exception ex)
        {
            logger.LogError(ex,
                "Unexpected error while creating material category for Name {Name}, Description {Description}, IsActive {IsActive}",
                resource.Name, resource.Description, resource.IsActive);

            return Problem(
                title: localizer["UnexpectedServerError"].Value,
                detail: localizer["UnexpectedErrorCreatingMaterialCategory"].Value,
                statusCode: 500);
        }
    }

    /// <summary>
    /// Gets a material category by ID
    /// </summary>
    /// <param name="id">The material category identifier</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The material category if found</returns>
    [HttpGet("{id}")]
    [SwaggerOperation(
        Summary = "Gets a Material Category by id",
        Description = "Gets a material category for a given material category identifier",
        OperationId = "GetMaterialCategoryById")]
    [SwaggerResponse(200, "The material category was found", typeof(MaterialCategoryResource))]
    [SwaggerResponse(404, "The material category was not found")]
    public async Task<ActionResult> GetMaterialCategoryById(int id, CancellationToken cancellationToken = default)
    {
        var getMaterialCategoryByIdQuery = new GetMaterialCategoryByIdQuery(id);
        var result = await materialCategoryQueryService.Handle(getMaterialCategoryByIdQuery, cancellationToken);
        if (result is null) return NotFound(localizer["MaterialCategoryNotFound"].Value);
        var resource = MaterialCategoryResourceFromEntityAssembler.ToResourceFromEntity(result);
        return Ok(resource);
    }
}