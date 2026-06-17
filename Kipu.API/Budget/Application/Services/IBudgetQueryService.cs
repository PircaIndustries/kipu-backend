using Kipu.API.Budget.Domain.Model.Aggregates;

namespace Kipu.API.Budget.Application.Services;

/// <summary>
/// Application service contract for handling inbound read operations (queries) for the Budget context.
/// </summary>
public interface IBudgetQueryService
{
    /// <summary>
    /// Retrieves a specific budget line item by its unique identifier.
    /// </summary>
    Task<BudgetItem?> HandleGetByIdAsync(int id);

    /// <summary>
    /// Retrieves all budget line items mapped to a given project identifier.
    /// </summary>
    Task<IEnumerable<BudgetItem>> HandleListByProjectIdAsync(int projectId);
}