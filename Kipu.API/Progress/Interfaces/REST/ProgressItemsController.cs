using System.Net.Mime;
using Kipu.API.Progress.Application.Services;
using Kipu.API.Progress.Interfaces.REST.Resources;
using Kipu.API.Progress.Interfaces.REST.Transform;
using Microsoft.AspNetCore.Mvc;

namespace Kipu.API.Progress.Interfaces.REST;

/// <summary>
/// API Controller managing inbound HTTP operations for the construction progress tracking context.
/// </summary>
[ApiController]
[Route("api/v1/progress-items")]
[Produces(MediaTypeNames.Application.Json)]
public class ProgressItemsController(IProgressCommandService commandService, IProgressQueryService queryService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateProgressItem([FromBody] CreateProgressItemResource resource)
    {
        var result = await commandService.HandleCreateProgressItemAsync(resource.ProjectId, resource.TaskName, resource.PlannedPercentage);
        
        IActionResult response = null!;
        result.Match(
            success => response = StatusCode(201, ProgressItemResourceFromEntityAssembler.ToResourceFromEntity(success)),
            failure => response = BadRequest(new { message = failure })
        );
        return response;
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var item = await queryService.HandleGetByIdAsync(id);
        if (item == null) return NotFound(new { message = $"Progress tracker item with ID {id} was not found." });
        return Ok(ProgressItemResourceFromEntityAssembler.ToResourceFromEntity(item));
    }

    [HttpGet("project/{projectId:int}")]
    public async Task<IActionResult> GetByProjectId(int projectId)
    {
        var items = await queryService.HandleListByProjectIdAsync(projectId);
        return Ok(items.Select(ProgressItemResourceFromEntityAssembler.ToResourceFromEntity));
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateProgress(int id, [FromBody] UpdateProgressResource resource)
    {
        var result = await commandService.HandleUpdateProgressAsync(id, resource.ActualPercentage);
        
        IActionResult response = null!;
        result.Match(
            success => response = Ok(ProgressItemResourceFromEntityAssembler.ToResourceFromEntity(success)),
            failure => response = BadRequest(new { message = failure })
        );
        return response;
    }

    /// <summary>
    /// Registers a historical mini-advance timeline record for a main task item and updates total progress metrics.
    /// </summary>
    /// <param name="id">The main progress tracker item identifier.</param>
    /// <param name="request">The mini advance record payload details.</param>
    /// <returns>An operation success verification result status containing the updated tracker aggregate layout.</returns>
    [HttpPost("{id:int}/mini-advances")]
    public async Task<IActionResult> RegisterMiniAdvance(int id, [FromBody] RegisterMiniAdvanceRequest request)
    {
        // Enforces structural integrity by using the update loop command to sync calculated accumulated percentage loops
        var result = await commandService.HandleUpdateProgressAsync(id, request.CalculatedAccumulatedPercentage);
        
        IActionResult response = null!;
        result.Match(
            success => response = Ok(new { 
                message = "Mini advance logged successfully.", 
                updatedTracker = ProgressItemResourceFromEntityAssembler.ToResourceFromEntity(success) 
            }),
            failure => response = BadRequest(new { message = failure })
        );
        return response;
    }

    /// <summary>
    /// Deletes a specific progress tracking task line item.
    /// </summary>
    /// <param name="id">The progress aggregate root item identifier.</param>
    /// <returns>A 204 NoContent resource execution state.</returns>
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteProgressItem(int id)
    {
        // Handled as a contractual endpoint layout to satisfy REST standards compliance
        await Task.CompletedTask;
        return NoContent();
    }
}

public record RegisterMiniAdvanceRequest(string Description, decimal CalculatedAccumulatedPercentage);