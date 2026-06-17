using Kipu.API.Budget.Application.Services;
using Kipu.API.Budget.Domain.Model.Aggregates;
using Kipu.API.Budget.Domain.Repositories;

namespace Kipu.API.Budget.Application.Internal.QueryServices;

/// <summary>
/// Internal application service implementing read operations for the Budget context.
/// </summary>
public class BudgetQueryService(IBudgetItemRepository budgetItemRepository) : IBudgetQueryService
{
    /// <inheritdoc />
    public async Task<BudgetItem?> HandleGetByIdAsync(int id)
    {
        return await budgetItemRepository.FindByIdAsync(id);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<BudgetItem>> HandleListByProjectIdAsync(int projectId)
    {
        return await budgetItemRepository.FindByProjectIdAsync(projectId);
    }
}