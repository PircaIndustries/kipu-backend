using System.Net.Mime;
using Kipu.API.Logistics.Application.Errors;
using Kipu.API.Logistics.Application.Services;
using Kipu.API.Logistics.Domain.Model.Aggregates;
using Kipu.API.Logistics.Domain.Model.Commands;
using Kipu.API.Logistics.Domain.Model.Queries;
using Kipu.API.Logistics.Domain.Model.ValueObjects;
using Kipu.API.Logistics.Interfaces.REST.Resources.Supplier;
using Kipu.API.Logistics.Interfaces.REST.Transform.Supplier;
using Kipu.API.Resources;
using Kipu.API.Shared.Application.Patterns;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Swashbuckle.AspNetCore.Annotations;

namespace Kipu.API.Logistics.Interfaces.REST;

[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[Tags("Supplier")]
public class SupplierController(
    ISupplierCommandService supplierCommandService,
    ISupplierQueryService supplierQueryService,
    IStringLocalizer<SharedResource> localizer,
    ILogger<SupplierController> logger) : ControllerBase
{
    /// <summary>
    /// Creates a new supplier
    /// </summary>
    /// <param name="resource">The supplier data (Ruc, SocialReason, Contact, Phone, Email, PaymentTerms)</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The created supplier</returns>
    [HttpPost]
    [SwaggerOperation(
        Summary = "Creates a Supplier",
        Description = "Creates a supplier with the given RUC, business name, contact, phone, email and payment terms",
        OperationId = "CreateSupplier")]
    [SwaggerResponse(201, "The supplier was created", typeof(SupplierResource))]
    [SwaggerResponse(400, "The request payload is invalid", typeof(string))]
    [SwaggerResponse(409, "The supplier already exists", typeof(string))]
    [SwaggerResponse(500, "Unexpected server error", typeof(ProblemDetails))]
    public async Task<ActionResult> CreateSupplier(
        [FromBody] CreateSupplierResource resource,
        CancellationToken cancellationToken)
    {
        try
        {
            var command = CreateSupplierCommandFromResourceAssembler.ToCommandFromResource(resource);
            var result = await supplierCommandService.Handle(command, cancellationToken);
            return ActionResultFromCreateSupplierAssembler.ToActionResultFromCreateSupplierResult(
                result,
                this,
                localizer,
                nameof(GetSupplierById));
        }
        catch (ArgumentException ex)
        {
            logger.LogWarning(ex,
                "Validation failed while creating supplier for Ruc {Ruc}, SocialReason {SocialReason}, Email {Email}",
                resource.Ruc, resource.SocialReason, resource.Email);
            return BadRequest(localizer["InvalidSupplierRequest"].Value);
        }
        catch (Exception ex)
        {
            logger.LogError(ex,
                "Unexpected error while creating supplier for Ruc {Ruc}, SocialReason {SocialReason}, Email {Email}",
                resource.Ruc, resource.SocialReason, resource.Email);

            return Problem(
                title: localizer["UnexpectedServerError"].Value,
                detail: localizer["UnexpectedErrorCreatingSupplier"].Value,
                statusCode: 500);
        }
    }

    /// <summary>
    /// Gets a supplier by ID
    /// </summary>
    /// <param name="id">The supplier identifier</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The supplier if found</returns>
    [HttpGet("{id}")]
    [SwaggerOperation(
        Summary = "Gets a Supplier by id",
        Description = "Gets a supplier for a given supplier identifier",
        OperationId = "GetSupplierById")]
    [SwaggerResponse(200, "The supplier was found", typeof(SupplierResource))]
    [SwaggerResponse(404, "The supplier was not found")]
    public async Task<ActionResult> GetSupplierById(int id, CancellationToken cancellationToken = default)
    {
        var query = new GetSupplierByIdQuery(id);
        var result = await supplierQueryService.Handle(query, cancellationToken);
        if (result is null)
            return NotFound(localizer["SupplierNotFound"].Value);
        var resource = SupplierResourceFromEntityAssembler.ToResourceFromEntity(result);
        return Ok(resource);
    }

    /// <summary>
    /// Gets all suppliers
    /// </summary>
    [HttpGet]
    [SwaggerOperation(
        Summary = "Gets all Suppliers",
        Description = "Retrieves all suppliers",
        OperationId = "GetAllSuppliers")]
    [SwaggerResponse(200, "The suppliers were found", typeof(IEnumerable<SupplierResource>))]
    public async Task<ActionResult> GetAllSuppliers(CancellationToken cancellationToken = default)
    {
        var suppliers = await supplierQueryService.Handle(new GetAllSuppliersQuery(), cancellationToken);
        var resources = suppliers.Select(SupplierResourceFromEntityAssembler.ToResourceFromEntity);
        return Ok(resources);
    }

    /// <summary>
    /// Gets a supplier by RUC (tax identification number)
    /// </summary>
    /// <param name="ruc">The supplier's RUC (11 digits)</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The supplier if found</returns>
    [HttpGet("ruc/{ruc}")]
    [SwaggerOperation(
        Summary = "Gets a Supplier by RUC",
        Description = "Gets a supplier for a given RUC (tax identification number)",
        OperationId = "GetSupplierByRuc")]
    [SwaggerResponse(200, "The supplier was found", typeof(SupplierResource))]
    [SwaggerResponse(404, "The supplier was not found")]
    public async Task<ActionResult> GetSupplierByRuc(Ruc ruc, CancellationToken cancellationToken = default)
    {
        var query = new GetAllSupplierByRucQuery(ruc);
        var result = await supplierQueryService.Handle(query, cancellationToken);
        if (result is null)
            return NotFound(localizer["SupplierNotFoundByRuc"].Value);
        var resource = SupplierResourceFromEntityAssembler.ToResourceFromEntity(result);
        return Ok(resource);
    }

    /// <summary>
    /// Gets all suppliers by active status
    /// </summary>
    /// <param name="isActive">Active status (true/false)</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A list of suppliers matching the active status</returns>
    [HttpGet("isactive/{isActive}")]
    [SwaggerOperation(
        Summary = "Gets all Suppliers by active status",
        Description = "Retrieves all suppliers that are active (true) or inactive (false)",
        OperationId = "GetSuppliersByIsActive")]
    [SwaggerResponse(200, "The suppliers were found", typeof(IEnumerable<SupplierResource>))]
    public async Task<ActionResult> GetSuppliersByIsActive(bool isActive, CancellationToken cancellationToken = default)
    {
        var query = new GetAllSupplierByIsActiveQuery(isActive);
        var result = await supplierQueryService.Handle(query, cancellationToken);
        var resources = result.Select(SupplierResourceFromEntityAssembler.ToResourceFromEntity);
        return Ok(resources);
    }
    
    /// <summary>
    /// Updates an existing supplier
    /// </summary>
    /// <param name="id">The supplier identifier</param>
    /// <param name="resource">The updated supplier data (all fields required)</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The updated supplier</returns>
    [HttpPut("{id}")]
    [SwaggerOperation(
        Summary = "Updates a Supplier",
        Description = "Updates an existing supplier with the given ID. All fields are required (PUT semantics).",
        OperationId = "UpdateSupplier")]
    [SwaggerResponse(200, "The supplier was updated", typeof(SupplierResource))]
    [SwaggerResponse(400, "The request payload is invalid", typeof(string))]
    [SwaggerResponse(404, "The supplier was not found", typeof(string))]
    [SwaggerResponse(409, "The RUC already exists for another supplier", typeof(string))]
    [SwaggerResponse(500, "Unexpected server error", typeof(ProblemDetails))]
    public async Task<ActionResult> UpdateSupplier(int id,     [FromBody] UpdateSupplierResource resource,
        CancellationToken cancellationToken)
    {
        try
        {
            var command = UpdateSupplierCommandFromResourceAssembler.ToCommandFromResource(id, resource);
            var result = await supplierCommandService.Handle(command, cancellationToken);
            return ActionResultFromUpdateSupplierAssembler.ToActionResultFromUpdateSupplierResult(
                result, this, localizer);
        }
        catch (ArgumentException ex)
        {
            logger.LogWarning(ex, "Validation failed while updating supplier with id {Id}", id);
            return BadRequest(localizer["InvalidSupplierRequest"].Value);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error while updating supplier with id {Id}", id);
            return Problem(
                title: localizer["UnexpectedServerError"].Value,
                detail: localizer["UnexpectedErrorUpdatingSupplier"].Value,
                statusCode: 500);
        }
    }
    
    /// <summary>
    /// Deletes an existing supplier
    /// </summary>
    /// <param name="id">The supplier identifier</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>No content if deleted</returns>
    [HttpDelete("{id}")]
    [SwaggerOperation(
        Summary = "Deletes a Supplier",
        Description = "Deletes an existing supplier with the given ID.",
        OperationId = "DeleteSupplier")]
    [SwaggerResponse(204, "The supplier was deleted")]
    [SwaggerResponse(404, "The supplier was not found", typeof(string))]
    [SwaggerResponse(500, "Unexpected server error", typeof(ProblemDetails))]
    public async Task<ActionResult> DeleteSupplier(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await supplierCommandService.HandleDelete(id, cancellationToken);
            if (result is Result<Supplier, UpdateSupplierError>.Success)
                return NoContent();
            return NotFound(localizer["SupplierNotFound"].Value);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error deleting supplier with ID {Id}", id);
            return Problem(
                title: localizer["UnexpectedServerError"].Value,
                detail: localizer["UnexpectedErrorDeletingSupplier"].Value,
                statusCode: 500);
        }
    }

    /// <summary>
    /// Updates Partial an existing supplier
    /// </summary>
    /// <param name="id">The supplier identifier</param>
    /// <param name="resource">The updated supplier data (all fields required)</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The updated Partial supplier</returns>
    [HttpPatch("{id}")]
    [SwaggerOperation(
        Summary = "Updates Partial a Supplier",
        Description = "Updates Partial an existing supplier with the given ID. All fields are required (PUT semantics).",
        OperationId = "UpdatePartialSupplier")]
    [SwaggerResponse(200, "The supplier was updated", typeof(SupplierResource))]
    [SwaggerResponse(400, "The request payload is invalid", typeof(string))]
    [SwaggerResponse(404, "The supplier was not found", typeof(string))]
    [SwaggerResponse(409, "The RUC already exists for another supplier", typeof(string))]
    [SwaggerResponse(500, "Unexpected server error", typeof(ProblemDetails))]
    public async Task<ActionResult> UpdatePartialSupplier(int id,     [FromBody] UpdatePartialSupplierResource resource,
        CancellationToken cancellationToken)
    {
        try
        {
            var command = UpdatePartialSupplierCommandFromResourceAssembler.ToCommandFromResource(id, resource);
            var result = await supplierCommandService.Handle(command, cancellationToken);
            return ActionResultFromUpdatePartialSupplierAssembler.ToActionResultFromUpdatePartialSupplierResult(
                result, this, localizer);
        }
        catch (ArgumentException ex)
        {
            logger.LogWarning(ex, "Validation failed while updating supplier with id {Id}", id);
            return BadRequest(localizer["InvalidSupplierRequest"].Value);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error while updating supplier with id {Id}", id);
            return Problem(
                title: localizer["UnexpectedServerError"].Value,
                detail: localizer["UnexpectedErrorUpdatingSupplier"].Value,
                statusCode: 500);
        }
    }
}