using Kipu.API.Budget.Domain.Model.Aggregates;
using Kipu.API.Shared.Application.Patterns;

namespace Kipu.API.Budget.Application.Services;

/// <summary>
/// Application service contract for handling inbound write operations (commands) for the Budget context.
/// </summary>
public interface IBudgetCommandService
{
    /// <summary>
    /// Handles the creation of a new budget line item.
    /// </summary>
    /// <param name="projectId">The target project identifier.</param>
    /// <param name="activityName">The unique name of the activity or budget line.</param>
    /// <param name="details">The descriptive details or sector specification.</param>
    /// <param name="assignedBudget">The initial allocated budget amount.</param>
    /// <returns>A result containing the created BudgetItem aggregate or an error message.</returns>
    Task<Result<BudgetItem, string>> HandleCreateBudgetItemAsync(int projectId, string activityName, string details, decimal assignedBudget);

    /// <summary>
    /// Handles registering a new expense transaction against an existing budget item.
    /// </summary>
    /// <param name="budgetItemId">The target budget item identifier.</param>
    /// <param name="amount">The expense amount to deduct.</param>
    /// <param name="description">The purpose or rationale of the transaction.</param>
    /// <returns>A result containing the updated BudgetItem aggregate or an error message.</returns>
    Task<Result<BudgetItem, string>> HandleRegisterExpenseAsync(int budgetItemId, decimal amount, string description);

    /// <summary>
    /// Handles budget line extension requests to increase total available funds.
    /// </summary>
    /// <param name="budgetItemId">The target budget item identifier.</param>
    /// <param name="additionalAmount">The extra funds to annex.</param>
    /// <returns>A result containing the updated BudgetItem aggregate or an error message.</returns>
    Task<Result<BudgetItem, string>> HandleRequestExtensionAsync(int budgetItemId, decimal additionalAmount);
}