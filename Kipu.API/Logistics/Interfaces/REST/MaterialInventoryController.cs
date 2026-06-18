using System.Net.Mime;
using Kipu.API.Logistics.Application.Errors;
using Kipu.API.Logistics.Application.Services;
using Kipu.API.Logistics.Domain.Model.Aggregates;
using Kipu.API.Logistics.Domain.Model.Queries;
using Kipu.API.Logistics.Domain.Model.ValueObjects;
using Kipu.API.Logistics.Interfaces.REST.Resources;
using Kipu.API.Logistics.Interfaces.REST.Transform;
using Kipu.API.Resources;
using Kipu.API.Shared.Application.Patterns;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Swashbuckle.AspNetCore.Annotations;
namespace Kipu.API.Logistics.Interfaces.REST;

[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[Tags("Material Inventories")]
public class MaterialInventoryController(
    IMaterialInventoryCommandService materialInventoryCommandService,
    IMaterialInventoryQueryService materialInventoryQueryService,
    IStringLocalizer<SharedResource> localizer,
    ILogger<MaterialInventoryController> logger) : ControllerBase
{
    [HttpPost]
    [SwaggerOperation(
        Summary = "Creates a Material Inventory",
        Description = "Creates a Material Inventory with a given ProjectId, MaterialId, CurrentStock, MinimumStock and Location",
        OperationId = "CreateMaterialInventory")]
    [SwaggerResponse(201, "The material inventory was created", typeof(MaterialInventoryResource))]
    [SwaggerResponse(400, "The request payload is invalid", typeof(string))]
    [SwaggerResponse(409, "The material inventory already exists", typeof(string))]
    [SwaggerResponse(500, "Unexpected server error", typeof(ProblemDetails))]
    public async Task<ActionResult> CreateMaterialInventory([FromBody] CreateMaterialInventoryResource resource,
        CancellationToken cancellationToken)
    {
        try
        {
            var createMaterialInventoryCommand =
                CreateMaterialInventoryCommandFromResourceAssembler.ToCommandFromResource(resource);
            var result = await materialInventoryCommandService.Handle(createMaterialInventoryCommand, cancellationToken);
            return ActionResultFromCreateMaterialInventoryAssembler.ToActionResultFromCreateMaterialInventoryResult(
                result,
                this,
                localizer,
                nameof(GetMaterialInventoryById));
        }
        catch (ArgumentException ex)
        {
            logger.LogWarning(ex,
                "Validation failed while creating material inventory for {MaterialId}, ProjectId {ProjectId}, CurrentStock {CurrentStock} and MinimumStock {MinimumStock}",
                resource.MaterialId,  resource.ProjectId, resource.CurrentStock, resource.MinimumStock);
            return BadRequest(localizer["InvalidMaterialInventoryRequest"].Value);
        }
        catch (Exception ex)
        {
            logger.LogError(ex,
                "Unexpected error while creating material inventory for {MaterialId}, ProjectId {ProjectId}, CurrentStock {CurrentStock} and MinimumStock {MinimumStock}",
                resource.MaterialId,  resource.ProjectId, resource.CurrentStock, resource.MinimumStock);

            return Problem(
                title: localizer["UnexpectedServerError"].Value,
                detail: localizer["UnexpectedErrorCreatingMaterialInventory"].Value,
                statusCode: 500);
        }
    }
    [HttpGet("category/{categoryId}")]
    [SwaggerOperation(
        Summary = "Gets all Material Inventories by Category Id",
        Description = "Retrieves all material inventories associated with a specific category identifier.",
        OperationId = "GetAllMaterialInventoryByCategoryId")]
    [SwaggerResponse(200, "The material inventories were found", typeof(IEnumerable<MaterialInventoryResource>))]
    public async Task<ActionResult> GetAllMaterialInventoryByCategoryId(CategoryId categoryId,
        CancellationToken cancellationToken)
    {
        var getAllMaterialInventoryByCategoryIdQuery = new GetAllMaterialInventoryByCategoryIdQuery(categoryId);
        var result = await materialInventoryQueryService.Handle(getAllMaterialInventoryByCategoryIdQuery, cancellationToken);
        var resources = result.Select(MaterialInventoryResourceFromEntityAssembler.ToResourceFromEntity);
        return Ok(resources);
    }
    [HttpGet("{id}")]
    [SwaggerOperation(
        Summary = "Gets a Material Inventory by id",
        Description = "Gets a material inventory for a given material inventory identifier",
        OperationId = "GetMaterialInventoryById")]
    [SwaggerResponse(200, "The material inventory was found", typeof(MaterialInventoryResource))]
    [SwaggerResponse(404, "The material inventory was not found")]
    public async Task<ActionResult> GetMaterialInventoryById(int id, CancellationToken cancellationToken = default)
    {
        var getMaterialInventoryByIdQuery = new GetMaterialInventoryByIdQuery(id);
        var result = await materialInventoryQueryService.Handle(getMaterialInventoryByIdQuery, cancellationToken);
        if (result is null) return NotFound();
        var resource = MaterialInventoryResourceFromEntityAssembler.ToResourceFromEntity(result);
        return Ok(resource);
    }
    
    [HttpGet]
    [SwaggerOperation(
        Summary = "Gets all Material Inventory",
        Description = "Gets all Material Inventory",
        OperationId = "GetAllMaterialInventory")]
    [SwaggerResponse(200, "The material Inventory were found", typeof(IEnumerable<MaterialInventoryResource>))]
    public async Task<ActionResult> GetAllMaterialInventory(CancellationToken cancellationToken = default)
    {
        var query = new GetAllMaterialInventoryQuery();
        var result = await materialInventoryQueryService.Handle(query, cancellationToken);
        var resources = result.Select(MaterialInventoryResourceFromEntityAssembler.ToResourceFromEntity);
        return Ok(resources);
    }

    /// <summary>
    /// Updates a material inventory
    /// </summary>
    /// <param name="id">The material inventory identifier</param>
    /// <param name="resource">The updated material inventory data</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The updated material inventory</returns>
    [HttpPut("{id}")]
    [SwaggerOperation(
        Summary = "Updates a Material Inventory",
        Description = "Updates an existing material inventory with the given ID",
        OperationId = "UpdateMaterialInventory")]
    [SwaggerResponse(200, "The material inventory was updated", typeof(MaterialInventoryResource))]
    [SwaggerResponse(400, "The request payload is invalid", typeof(string))]
    [SwaggerResponse(404, "The material inventory was not found", typeof(string))]
    [SwaggerResponse(500, "Unexpected server error", typeof(ProblemDetails))]
    public async Task<ActionResult> UpdateMaterialInventory(int id,
        [FromBody] UpdateMaterialInventoryResource resource,
        CancellationToken cancellationToken)
    {
        try
        {
            var command = UpdateMaterialInventoryCommandFromResourceAssembler.ToCommandFromResource(id, resource);
            var result = await materialInventoryCommandService.Handle(command, cancellationToken);
            return ActionResultFromUpdateMaterialInventoryAssembler.ToActionResultFromUpdateMaterialInventoryResult(
                result, this, localizer);
        }
        catch (ArgumentException ex)
        {
            logger.LogWarning(ex, "Validation failed while updating material inventory with id {Id}", id);
            return BadRequest(localizer["InvalidMaterialInventoryRequest"].Value);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error while updating material inventory with id {Id}", id);
            return Problem(
                title: localizer["UnexpectedServerError"].Value,
                detail: localizer["UnexpectedErrorUpdatingMaterialInventory"].Value,
                statusCode: 500);
        }
    }

    /// <summary>
    /// Deletes a material inventory
    /// </summary>
    /// <param name="id">The material inventory identifier</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>No content if successful</returns>
    [HttpDelete("{id}")]
    [SwaggerOperation(
        Summary = "Deletes a Material Inventory",
        Description = "Deletes an existing material inventory with the given ID",
        OperationId = "DeleteMaterialInventory")]
    [SwaggerResponse(204, "The material inventory was deleted")]
    [SwaggerResponse(404, "The material inventory was not found", typeof(string))]
    [SwaggerResponse(500, "Unexpected server error", typeof(ProblemDetails))]
    public async Task<ActionResult> DeleteMaterialInventory(int id,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await materialInventoryCommandService.HandleDelete(id, cancellationToken);
            if (result is Result<MaterialInventory, UpdateMaterialInventoryError>.Success)
                return NoContent();
            var failure = (Result<MaterialInventory, UpdateMaterialInventoryError>.Failure)result;
            return failure.Error switch
            {
                UpdateMaterialInventoryError.MaterialInventoryNotFound =>
                    NotFound(localizer["MaterialInventoryNotFound"].Value),
                _ => Problem(
                    title: localizer["UnexpectedServerError"].Value,
                    detail: localizer["UnexpectedErrorDeletingMaterialInventory"].Value,
                    statusCode: 500)
            };
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error while deleting material inventory with id {Id}", id);
            return Problem(
                title: localizer["UnexpectedServerError"].Value,
                detail: localizer["UnexpectedErrorDeletingMaterialInventory"].Value,
                statusCode: 500);
        }
    }
}