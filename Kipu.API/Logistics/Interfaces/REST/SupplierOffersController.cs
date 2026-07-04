using System.Net.Mime;
using Kipu.API.Logistics.Application.Errors;
using Kipu.API.Logistics.Application.Services;
using Kipu.API.Logistics.Domain.Model.Aggregates;
using Kipu.API.Logistics.Domain.Model.Queries;
using Kipu.API.Logistics.Interfaces.REST.Resources.SupplierOffer;
using Kipu.API.Logistics.Interfaces.REST.Transform.SupplierOffer;
using Kipu.API.Resources;
using Kipu.API.Shared.Application.Patterns;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Swashbuckle.AspNetCore.Annotations;

namespace Kipu.API.Logistics.Interfaces.REST;

[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[Tags("Supplier Offers")]
public class SupplierOffersController(
    ISupplierOfferCommandService supplierOfferCommandService,
    ISupplierOfferQueryService supplierOfferQueryService,
    IStringLocalizer<SharedResource> localizer,
    ILogger<SupplierOffersController> logger) : ControllerBase
{
    [HttpPost]
    [SwaggerOperation(Summary = "Creates a Supplier Offer")]
    [SwaggerResponse(201, "Created", typeof(SupplierOfferResource))]
    [SwaggerResponse(400, "Invalid request", typeof(string))]
    [SwaggerResponse(409, "Duplicate", typeof(string))]
    public async Task<ActionResult> CreateSupplierOffer([FromBody] CreateSupplierOfferResource resource,
        CancellationToken cancellationToken)
    {
        try
        {
            var command = CreateSupplierOfferCommandFromResourceAssembler.ToCommandFromResource(resource);
            var result = await supplierOfferCommandService.Handle(command, cancellationToken);
            return ActionResultFromCreateSupplierOfferAssembler.ToActionResultFromCreateSupplierOfferResult(
                result, this, localizer, nameof(GetSupplierOfferById));
        }
        catch (ArgumentException ex)
        {
            logger.LogWarning(ex, "Validation failed");
            return BadRequest(localizer["InvalidSupplierOfferRequest"].Value);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error");
            return Problem(title: localizer["UnexpectedServerError"].Value, statusCode: 500);
        }
    }

    [HttpGet("{id}")]
    [SwaggerOperation(Summary = "Gets a Supplier Offer by id")]
    [SwaggerResponse(200, "Found", typeof(SupplierOfferResource))]
    [SwaggerResponse(404, "Not found")]
    public async Task<ActionResult> GetSupplierOfferById(int id, CancellationToken cancellationToken = default)
    {
        var query = new GetSupplierOfferByIdQuery(id);
        var result = await supplierOfferQueryService.Handle(query, cancellationToken);
        if (result is null) return NotFound();
        return Ok(SupplierOfferResourceFromEntityAssembler.ToResourceFromEntity(result));
    }

    [HttpGet]
    [SwaggerOperation(Summary = "Gets all Supplier Offers")]
    [SwaggerResponse(200, "Found", typeof(IEnumerable<SupplierOfferResource>))]
    public async Task<ActionResult> GetAllSupplierOffers(CancellationToken cancellationToken = default)
    {
        var query = new GetAllSupplierOffersQuery();
        var result = await supplierOfferQueryService.Handle(query, cancellationToken);
        var resources = result.Select(SupplierOfferResourceFromEntityAssembler.ToResourceFromEntity);
        return Ok(resources);
    }

    [HttpGet("by-supplier/{supplierId}")]
    [SwaggerOperation(Summary = "Gets Supplier Offers by supplier ID")]
    [SwaggerResponse(200, "Found", typeof(IEnumerable<SupplierOfferResource>))]
    public async Task<ActionResult> GetSupplierOffersBySupplierId(int supplierId, CancellationToken cancellationToken = default)
    {
        var query = new GetSupplierOffersBySupplierIdQuery(new Domain.Model.ValueObjects.SupplierId(supplierId));
        var result = await supplierOfferQueryService.Handle(query, cancellationToken);
        var resources = result.Select(SupplierOfferResourceFromEntityAssembler.ToResourceFromEntity);
        return Ok(resources);
    }

    [HttpGet("by-material/{materialId}")]
    [SwaggerOperation(Summary = "Gets Supplier Offers by material ID")]
    [SwaggerResponse(200, "Found", typeof(IEnumerable<SupplierOfferResource>))]
    public async Task<ActionResult> GetSupplierOffersByMaterialId(int materialId, CancellationToken cancellationToken = default)
    {
        var query = new GetSupplierOffersByMaterialIdQuery(new Domain.Model.ValueObjects.MaterialCatalogId(materialId));
        var result = await supplierOfferQueryService.Handle(query, cancellationToken);
        var resources = result.Select(SupplierOfferResourceFromEntityAssembler.ToResourceFromEntity);
        return Ok(resources);
    }

    [HttpPut("{id}")]
    [SwaggerOperation(Summary = "Updates a Supplier Offer")]
    [SwaggerResponse(200, "Updated", typeof(SupplierOfferResource))]
    [SwaggerResponse(400, "Invalid request", typeof(string))]
    [SwaggerResponse(404, "Not found", typeof(string))]
    public async Task<ActionResult> UpdateSupplierOffer(int id, [FromBody] UpdateSupplierOfferResource resource,
        CancellationToken cancellationToken)
    {
        try
        {
            var command = UpdateSupplierOfferCommandFromResourceAssembler.ToCommandFromResource(id, resource);
            var result = await supplierOfferCommandService.Handle(command, cancellationToken);
            return ActionResultFromUpdateSupplierOfferAssembler.ToActionResultFromUpdateSupplierOfferResult(
                result, this, localizer);
        }
        catch (ArgumentException ex)
        {
            logger.LogWarning(ex, "Validation failed");
            return BadRequest(localizer["InvalidSupplierOfferRequest"].Value);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error");
            return Problem(title: localizer["UnexpectedServerError"].Value, statusCode: 500);
        }
    }

    [HttpDelete("{id}")]
    [SwaggerOperation(Summary = "Deletes a Supplier Offer")]
    [SwaggerResponse(204, "Deleted")]
    [SwaggerResponse(404, "Not found")]
    public async Task<ActionResult> DeleteSupplierOffer(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await supplierOfferCommandService.HandleDelete(id, cancellationToken);
            if (result is Result<SupplierOffer, UpdateSupplierOfferError>.Success)
                return NoContent();
            return NotFound(localizer["SupplierOfferNotFound"].Value);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error deleting supplier offer");
            return Problem(title: localizer["UnexpectedServerError"].Value, statusCode: 500);
        }
    }
}
