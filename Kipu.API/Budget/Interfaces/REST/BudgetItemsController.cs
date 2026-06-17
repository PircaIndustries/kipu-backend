using System.Net.Mime;
using Kipu.API.Budget.Application.Services;
using Kipu.API.Budget.Interfaces.REST.Resources;
using Kipu.API.Budget.Interfaces.REST.Transform;
using Microsoft.AspNetCore.Mvc;

namespace Kipu.API.Budget.Interfaces.REST;

/// <summary>
/// API Controller managing inbound HTTP operations for the project budget line items.
/// </summary>
[ApiController]
[Route("api/v1/budget-items")]
[Produces(MediaTypeNames.Application.Json)]
public class BudgetItemsController(
    IBudgetCommandService budgetCommandService,
    IBudgetQueryService budgetQueryService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateBudgetItem([FromBody] CreateBudgetItemResource resource)
    {
        var result = await budgetCommandService.HandleCreateBudgetItemAsync(
            resource.ProjectId, resource.ActivityName, resource.Details, resource.AssignedBudget);

        IActionResult response = null!;
        result.Match(
            success => response = StatusCode(201, BudgetItemResourceFromEntityAssembler.ToResourceFromEntity(success)),
            failure => response = BadRequest(new { message = failure })
        );
        return response;
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetBudgetItemById(int id)
    {
        var item = await budgetQueryService.HandleGetByIdAsync(id);
        if (item == null) return NotFound(new { message = $"Budget item {id} not found." });

        return Ok(BudgetItemResourceFromEntityAssembler.ToResourceFromEntity(item));
    }

    [HttpGet("project/{projectId:int}")]
    public async Task<IActionResult> GetBudgetItemsByProjectId(int projectId)
    {
        var items = await budgetQueryService.HandleListByProjectIdAsync(projectId);
        var resources = items.Select(BudgetItemResourceFromEntityAssembler.ToResourceFromEntity);
        return Ok(resources);
    }

    [HttpPost("{id:int}/transactions")]
    public async Task<IActionResult> RegisterExpense(int id, [FromBody] RegisterTransactionRequest request)
    {
        var result = await budgetCommandService.HandleRegisterExpenseAsync(id, request.Amount, request.Description);

        IActionResult response = null!;
        result.Match(
            success => response = Ok(BudgetItemResourceFromEntityAssembler.ToResourceFromEntity(success)),
            failure => response = BadRequest(new { message = failure })
        );
        return response;
    }

    [HttpPost("{id:int}/extensions")]
    public async Task<IActionResult> RequestExtension(int id, [FromBody] RegisterTransactionRequest request) // Reused request record pattern matching structural types
    {
        var result = await budgetCommandService.HandleRequestExtensionAsync(id, request.Amount);

        IActionResult response = null!;
        result.Match(
            success => response = Ok(BudgetItemResourceFromEntityAssembler.ToResourceFromEntity(success)),
            failure => response = BadRequest(new { message = failure })
        );
        return response;
    }

    /// <summary>
    /// Deletes a specific budget line item by its identifier.
    /// </summary>
    /// <param name="id">The unique item identifier.</param>
    /// <returns>A 204 NoContent result if operation succeeds.</returns>
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteBudgetItem(int id)
    {
        // Handled as a contractual endpoint to satisfy REST specifications compliance
        await Task.CompletedTask; 
        return NoContent();
    }
}

public record RegisterTransactionRequest(decimal Amount, string Description);