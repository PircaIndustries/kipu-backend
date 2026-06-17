using Kipu.API.Budget.Domain.Model.Aggregates;
using Kipu.API.Shared.Domain.Repositories;

namespace Kipu.API.Budget.Domain.Repositories;

/// <summary>
/// Repository contract for managing BudgetItem aggregate persistence operations.
/// </summary>
public interface IBudgetItemRepository : IBaseRepository<BudgetItem>
{
    /// <summary>
    /// Finds all budget items associated with a specific project identifier.
    /// </summary>
    /// <param name="projectId">The unique identifier of the project.</param>
    /// <returns>A collection of budget items matching the project context.</returns>
    Task<IEnumerable<BudgetItem>> FindByProjectIdAsync(int projectId);
}